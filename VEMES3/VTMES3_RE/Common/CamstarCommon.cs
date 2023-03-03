﻿using Camstar.XMLClient.API;
using DevExpress.CodeParser;
using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VTMES3_RE.Common
{
    public class CamstarCommon
    {
        csiClient gClient = new csiClient();
        csiConnection gConnection = null;
        csiSession gSession = null;
        csiDocument gDocument = null;
        csiService gService = null;
        string gStrSessionID = "";

        string gHost = WrGlobal.Camstar_Host;
        int gPort = WrGlobal.Camstar_Port;

        string gUserName = WrGlobal.Camstar_UserName;
        string gPassword = WrGlobal.Camstar_Password;

        CamstarMessage camMessage = new CamstarMessage();

        public bool IsExecuting = false;

        Database db = new Database();
        string query = "";

        public CamstarCommon()
        {
            try
            {
                gConnection = gClient.createConnection(gHost, gPort);
                //gSession = gConnection.createSession(gUserName, gPassword, gSessionID.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Camstar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CreateDocumentandService(string documentName, string serviceName)
        {
            if (documentName.Length > 0)
            {
                gSession.removeDocument(documentName);
            }

            if (gService != null)
            {
                gService = null;
            }

            gDocument = gSession.createDocument(documentName);

            if (serviceName.Length > 0)
            {
                gService = gDocument.createService(serviceName);
            }
        }

        public void PrintDoc(string strDoc, bool isInputDoc)
        {
            string strDocFileName = "";
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            if (isInputDoc)
            {
                strDocFileName = "inputDoc.xml";
            }
            else
            {
                strDocFileName = "responseDoc.xml";
            }

            if (File.Exists(path + "\\" + strDocFileName))
            {
                File.Delete(path + "\\" + strDocFileName);
            }

            File.WriteAllText(path + "\\" + strDocFileName, strDoc, Encoding.Default);
        }

        private void ErrorsCheck(csiDocument ResponseDocument)
        {
            csiExceptionData csiexceptiondata;

            if (ResponseDocument.checkErrors())
            {
                camMessage.Result = false;
                csiexceptiondata = ResponseDocument.exceptionData();
                camMessage.Message = "Severity: " + csiexceptiondata.getSeverity() + " Description: " + csiexceptiondata.getDescription();
            }
            else
            {
                camMessage.Result = true;
                camMessage.Message = "성공!";
            }
        }

        public string CreateSession()
        {
            try
            {
                gStrSessionID = Guid.NewGuid().ToString();
                gSession = gConnection.createSession(gUserName, gPassword, gStrSessionID);

                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DestroySession()
        {
            try
            {
                gConnection.removeSession(gStrSessionID);

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void DestroyConnection()
        {
            gConnection.removeSession(gStrSessionID);
            gClient.removeConnection(gHost, gPort);
        }

        #region Role Function
        public CamstarMessage RoleDelete(string employeeName, int idx)
        {
            csiDocument ResponseDocument = null;
            csiObject InputData = null;
            csiObject InputData2 = null;
            csiSubentity ObjectChanges = null;
            csiNamedSubentityList Roles = null;

            try
            {
                CreateDocumentandService("EmployeeMaintTrans", "EmployeeMaint");

                InputData = gService.inputData();
                InputData.namedObjectField("ObjectToChange").setRef(employeeName);

                gService.perform("Load");

                InputData2 = gService.inputData();

                ObjectChanges = InputData2.subentityField("ObjectChanges");
                Roles = ObjectChanges.namedSubentityList("Roles");

                Roles.deleteItemByIndex(idx);

                gService.setExecute();
                gService.requestData().requestField("CompletionMsg");

                PrintDoc(gDocument.asXML(), true);
                ResponseDocument = gDocument.submit();
                PrintDoc(ResponseDocument.asXML(), false);
                ErrorsCheck(ResponseDocument);
            }
            catch (Exception ex)
            {
                camMessage.Result = false;
                camMessage.Message = ex.Message;
            }

            return camMessage;
        }

        public CamstarMessage RoleAdd(string employeename, string roleName)
        {
            csiDocument ResponseDocument = null;
            csiObject InputData = null;
            csiObject InputData2 = null;
            csiSubentity ObjectChanges = null;
            csiSubentity Members = null;

            try
            {
                CreateDocumentandService("EmployeeMaintTrans", "EmployeeMaint");

                InputData = gService.inputData();
                InputData.namedObjectField("ObjectToChange").setRef(employeename);

                gService.perform("Load");

                InputData2 = gService.inputData();

                ObjectChanges = InputData2.subentityField("ObjectChanges");
                Members = ObjectChanges.subentityList("Roles").appendItem();
                Members.namedObjectField("Role").setRef(roleName);
                Members.dataField("PropagateToChildOrgs").setValue(false.ToString());

                gService.setExecute();
                gService.requestData().requestField("CompletionMsg");

                PrintDoc(gDocument.asXML(), true);
                ResponseDocument = gDocument.submit();
                PrintDoc(ResponseDocument.asXML(), false);
                ErrorsCheck(ResponseDocument);
            }
            catch (Exception ex)
            {
                camMessage.Result = false;
                camMessage.Message = ex.Message;
            }

            return camMessage;
        }
        #endregion

        #region Container Function

        public int ContainerStart(DataTable table)
        {
            int successCnt = 0;

            csiDocument ResponseDocument = null;
            csiObject InputData = null;
            csiSubentity Details = null;
            csiSubentity CurrentStatusDetails = null;

            try
            {
                CreateSession();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "세션 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            try
            {
                foreach (DataRow dr in table.Rows)
                {
                    // Set Service Type
                    CreateDocumentandService("StartDoc", "Start");

                    // Set Input Data
                    InputData = gService.inputData();
                    //Set CurrentStatusDetails
                    CurrentStatusDetails = InputData.subentityField("CurrentStatusDetails");
                    CurrentStatusDetails.revisionedObjectField("Workflow").setRef((dr["Workflow"] ?? "").ToString(), "", true);
                    // Set Start Details
                    Details = InputData.subentityField("Details");

                    //Set Auto Container Name
                    if ((dr["Container"] ?? "").ToString() == "Auto")
                    {
                        Details.dataField("AutoNumber").setValue("True");
                        Details.dataField("IsContainer").setValue("True");
                    }
                    else
                    {
                        Details.dataField("ContainerName").setValue((dr["Container"] ?? "").ToString());
                    }

                    // Set Start Element
                    Details.namedObjectField("Owner").setRef((dr["Owner"] ?? "").ToString());
                    Details.dataField("Qty").setValue((dr["Qty"] ?? "0").ToString());
                    Details.namedObjectField("StartReason").setRef((dr["StartReason"] ?? "").ToString());
                    Details.namedObjectField("UOM").setRef((dr["UOM"] ?? "").ToString());
                    Details.namedObjectField("Level").setRef((dr["Level"] ?? "").ToString());
                    Details.namedObjectField("PriorityCode").setRef((dr["PriorityCode"] ?? "").ToString());
                    Details.namedObjectField("MfgOrder").setRef((dr["MfgOrder"] ?? "").ToString());
                    Details.revisionedObjectField("Product").setRef((dr["Product"] ?? "").ToString(), "", true);
                    Details.dataField("ContainerComment").setValue((dr["Comments"] ?? "").ToString());

                    // Set Factory 
                    InputData.namedObjectField("Factory").setRef((dr["Factory"] ?? "").ToString());

                    // Service Excute and request Completion Msg
                    gService.setExecute();
                    gService.requestData().requestField("CompletionMsg");

                    // Print XMl Dcoument
                    PrintDoc(gDocument.asXML(), true);
                    ResponseDocument = gDocument.submit();
                    PrintDoc(ResponseDocument.asXML(), false);

                    ErrorsCheck(ResponseDocument);

                    dr.BeginEdit();
                    if (camMessage.Result)
                    {
                        csiService csiservice = ResponseDocument.getService();
                        if (csiservice != null && (dr["Container"] ?? "").ToString() == "Auto")
                        {
                            csiDataField csidatafield = (csiDataField)csiservice.responseData().getResponseFieldByName("CompletionMsg");
                            dr["Container"] = csidatafield.getValue().Split(new char[] { ' ' })[0].Trim();
                        }
                        successCnt++;
                    }
                    else
                    {
                        dr["Container"] = "";
                    }
                    dr["Result"] = camMessage.Message;
                    dr["BoolResult"] = camMessage.Result;
                    dr.EndEdit();
                }
            }
            catch (Exception ex)
            {
                camMessage.Result = false;
                camMessage.Message = ex.Message;
            }

            DestroySession();

            return successCnt;
        }

        public int ContainerStartWithAttributes(DataTable table)
        {
            int successCnt = 0;

            List<string> arrList = new List<string>();

            int startAttrIdx = table.Columns.IndexOf("Start Result");
            int endAttrIdx = table.Columns.IndexOf("Attribute Result");

            if (startAttrIdx > -1 || endAttrIdx > -1)
            {
                startAttrIdx = startAttrIdx + 1;
                endAttrIdx = endAttrIdx - 1;

                for (int i = startAttrIdx; i < endAttrIdx + 1; i++)
                {
                    arrList.Add(table.Columns[i].ColumnName);
                }
            }

            csiDocument ResponseDocument = null;
            csiObject InputData = null;
            csiSubentity Details = null;
            csiSubentity CurrentStatusDetails = null;

            try
            {
                CreateSession();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "세션 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            try
            {
                foreach (DataRow dr in table.Rows)
                {
                    // Set Service Type
                    CreateDocumentandService("StartDoc", "Start");

                    // Set Input Data
                    InputData = gService.inputData();
                    //Set CurrentStatusDetails
                    CurrentStatusDetails = InputData.subentityField("CurrentStatusDetails");
                    CurrentStatusDetails.revisionedObjectField("Workflow").setRef((dr["Workflow"] ?? "").ToString(), "", true);
                    // Set Start Details
                    Details = InputData.subentityField("Details");

                    //Set Auto Container Name
                    if ((dr["Container"] ?? "").ToString() == "Auto")
                    {
                        Details.dataField("AutoNumber").setValue("True");
                        Details.dataField("IsContainer").setValue("True");
                    }
                    else
                    {
                        Details.dataField("ContainerName").setValue((dr["Container"] ?? "").ToString());
                    }

                    // Set Start Element
                    Details.namedObjectField("Owner").setRef((dr["Owner"] ?? "").ToString());
                    Details.dataField("Qty").setValue((dr["Qty"] ?? "0").ToString());
                    Details.namedObjectField("StartReason").setRef((dr["StartReason"] ?? "").ToString());
                    Details.namedObjectField("UOM").setRef((dr["UOM"] ?? "").ToString());
                    Details.namedObjectField("Level").setRef((dr["Level"] ?? "").ToString());
                    Details.namedObjectField("PriorityCode").setRef((dr["PriorityCode"] ?? "").ToString());
                    Details.namedObjectField("MfgOrder").setRef((dr["MfgOrder"] ?? "").ToString());
                    Details.revisionedObjectField("Product").setRef((dr["Product"] ?? "").ToString(), "", true);
                    Details.dataField("ContainerComment").setValue((dr["Comments"] ?? "").ToString());

                    // Set Factory 
                    InputData.namedObjectField("Factory").setRef((dr["Factory"] ?? "").ToString());

                    // Service Excute and request Completion Msg
                    gService.setExecute();
                    gService.requestData().requestField("CompletionMsg");

                    // Print XMl Dcoument
                    PrintDoc(gDocument.asXML(), true);
                    ResponseDocument = gDocument.submit();
                    PrintDoc(ResponseDocument.asXML(), false);

                    ErrorsCheck(ResponseDocument);

                    dr.BeginEdit();
                    if (camMessage.Result)
                    {
                        csiService csiservice = ResponseDocument.getService();
                        if (csiservice != null && (dr["Container"] ?? "").ToString() == "Auto")
                        {
                            csiDataField csidatafield = (csiDataField)csiservice.responseData().getResponseFieldByName("CompletionMsg");
                            dr["Container"] = csidatafield.getValue().Split(new char[] { ' ' })[0].Trim();
                        }
                        successCnt++;
                    }
                    else
                    {
                        dr["Container"] = "";
                    }
                    dr["Start Result"] = camMessage.Message;
                    dr["BoolResult"] = camMessage.Result;
                    dr.EndEdit();

                    if (camMessage.Result && arrList.Count > 0)
                    {
                        ContainerAttribute(dr, arrList);
                    }
                }
            }
            catch (Exception ex)
            {
                camMessage.Result = false;
                camMessage.Message = ex.Message;
            }

            DestroySession();

            return successCnt;
        }

        public int StartTwoLevel(DataTable table)
        {
            int successCnt = 0;
            List<string> arrList = new List<string>();

            int startAttrIdx = table.Columns.IndexOf("Start Result");
            int endAttrIdx = table.Columns.IndexOf("Attribute Result");

            if (startAttrIdx > -1 || endAttrIdx > -1)
            {
                startAttrIdx = startAttrIdx + 1;
                endAttrIdx = endAttrIdx - 1;

                for (int i = startAttrIdx; i < endAttrIdx + 1; i++)
                {
                    arrList.Add(table.Columns[i].ColumnName);
                }
            }

            csiDocument ResponseDocument = null;
            csiObject InputData = null;
            csiSubentity Details = null;
            csiSubentity CurrentStatusDetails = null;
            csiSubentity ChildContainers = null;

            try
            {
                CreateSession();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "세션 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            try
            {
                foreach (DataRow dr in table.Rows)
                {
                    // Set Service Type
                    CreateDocumentandService("StartDoc", "Start");

                    // Set Input Data
                    InputData = gService.inputData();

                    // Set CurrentStatusDetails
                    CurrentStatusDetails = InputData.subentityField("CurrentStatusDetails");
                    CurrentStatusDetails.revisionedObjectField("Workflow").setRef((dr["Workflow"] ?? "").ToString(), "", true);

                    // Set Start Details
                    Details = InputData.subentityField("Details");

                    // Set Container Details
                    if ((dr["Container"] ?? "").ToString() == "Auto")
                    {
                        Details.dataField("AutoNumber").setValue("True");
                    }
                    else
                    {
                        Details.dataField("ContainerName").setValue((dr["Container"] ?? "").ToString());
                    }
                    Details.namedObjectField("Level").setRef((dr["Level"] ?? "").ToString());
                    Details.namedObjectField("Owner").setRef((dr["Owner"] ?? "").ToString());
                    Details.namedObjectField("StartReason").setRef((dr["StartReason"] ?? "").ToString());
                    Details.namedObjectField("MfgOrder").setRef((dr["MfgOrder"] ?? "").ToString());
                    Details.revisionedObjectField("Product").setRef((dr["Product"] ?? "").ToString(), "", true);
                    Details.namedObjectField("PriorityCode").setRef((dr["PriorityCode"] ?? "").ToString());
                    Details.dataField("ContainerComment").setValue((dr["Comments"] ?? "").ToString());

                    // Set Factory
                    InputData.namedObjectField("Factory").setRef((dr["Factory"] ?? "").ToString());

                    // Set Child Container info
                    Details.dataField("ChildAutoNumber").setValue("True");
                    Details.dataField("ChildCount").setValue((dr["ChildCount"] ?? "0").ToString());
                    Details.dataField("DefaultChildQty").setValue((dr["ChildQty"] ?? "0").ToString());

                    // Set CildContainers 
                    Int32 ChildCount = Int32.Parse((dr["ChildCount"]).ToString());
                    ;

                    for (int i = 1; i <= ChildCount; i++)
                    {
                        ChildContainers = Details.subentityList("ChildContainers").appendItem();
                        ChildContainers.dataField("ContainerName").setValue("");
                        ChildContainers.namedObjectField("Level").setRef((dr["ChildLevel"] ?? "").ToString());
                        ChildContainers.dataField("Qty").setValue((dr["ChildQty"] ?? "0").ToString());
                        ChildContainers.namedObjectField("UOM").setRef((dr["ChildUOM"] ?? "").ToString());
                    }

                    // Service Excute and request Completion Msg
                    gService.setExecute();
                    gService.requestData().requestField("CompletionMsg");
                    gService.requestData().requestField("ACEMessage");
                    gService.requestData().requestField("ACEStatus");

                    // Print XMl Dcoument
                    PrintDoc(gDocument.asXML(), true);
                    ResponseDocument = gDocument.submit();
                    PrintDoc(ResponseDocument.asXML(), false);

                    ErrorsCheck(ResponseDocument);

                    dr.BeginEdit();
                    if (camMessage.Result)
                    {
                        csiService csiservice = ResponseDocument.getService();
                        if (csiservice != null && (dr["Container"] ?? "").ToString() == "Auto")
                        {
                            csiDataField csidatafield = (csiDataField)csiservice.responseData().getResponseFieldByName("CompletionMsg");
                            dr["Container"] = csidatafield.getValue().Split(new char[] { ' ' })[0].Trim();
                        }
                        successCnt++;
                    }
                    else
                    {
                        dr["Container"] = "";
                    }

                    dr["Start Result"] = camMessage.Message;
                    dr["BoolResult"] = camMessage.Result;
                    dr.EndEdit();

                    if (camMessage.Result && arrList.Count > 0)
                    {
                        ContainerAttribute(dr, arrList);
                    }
                }
            }
            catch (Exception ex)
            {
                camMessage.Result = false;
                camMessage.Message = ex.Message;
            }

            DestroySession();

            return successCnt;
        }

        public int PrintContainerLabel(DataTable table)
        {
            int successCnt = 0;

            csiDocument ResponseDocument = null;
            csiObject InputData = null;

            try
            {
                CreateSession();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "세션 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            try
            {
                foreach (DataRow dr in table.Rows)
                {
                    // Set Service Type
                    CreateDocumentandService("PrintContainerLabelDoc", "PrintContainerLabel");

                    // Set Input Data
                    InputData = gService.inputData();
                    InputData.namedObjectField("Container").setRef((dr["Container"] ?? "").ToString());
                    InputData.dataField("LabelCount").setValue((dr["Label Count"] ?? "").ToString());

                    if (dr["Printer Label Rev"].ToString() == "")
                    {
                        InputData.revisionedObjectField("PrinterLabelDefinition").setRef((dr["Printer Label Definition"] ?? "").ToString(), "", true);
                    }
                    else
                    {
                        InputData.revisionedObjectField("PrinterLabelDefinition").setRef((dr["Printer Label Definition"] ?? "").ToString(), (dr["Printer Label Rev"] ?? "").ToString(), true);
                    }
                    InputData.namedObjectField("PrintQueue").setRef((dr["Print Queue"] ?? "").ToString());
                    InputData.namedObjectField("TaskContainer").setRef((dr["Container"] ?? "").ToString());

                    // Service Excute and request Completion Msg
                    gService.setExecute();
                    gService.requestData().requestField("CompletionMsg");

                    // Print XMl Dcoument
                    PrintDoc(gDocument.asXML(), true);
                    ResponseDocument = gDocument.submit();
                    PrintDoc(ResponseDocument.asXML(), false);

                    ErrorsCheck(ResponseDocument);

                    dr.BeginEdit();
                    if (camMessage.Result)
                    {
                        successCnt++;
                    }

                    dr["Label Print Result"] = camMessage.Message;
                    dr["BoolResult"] = camMessage.Result;
                    dr.EndEdit();
                }
            }
            catch (Exception ex)
            {
                camMessage.Result = false;
                camMessage.Message = ex.Message;
            }

            DestroySession();

            return successCnt;
        }

        public int PrintContainerTwoLabel(DataTable table)
        {
            int successCnt = 0;

            csiDocument ResponseDocument = null;
            csiObject InputData = null;

            try
            {
                CreateSession();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "세션 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            try
            {
                {
                foreach (DataRow dr in table.Rows)
                    for (int i = 0; i < Convert.ToInt32(dr["ChildCount"]) + 1; i++)
                    {
                        string containerName = (dr["Container"] ?? "").ToString();

                        if (i > 0)
                        {
                            containerName += i.ToString("00");
                        }

                        // Set Service Type
                        CreateDocumentandService("PrintContainerLabelDoc", "PrintContainerLabel");

                        // Set Input Data
                        InputData = gService.inputData();
                        InputData.namedObjectField("Container").setRef(containerName);
                        InputData.dataField("LabelCount").setValue((dr["Label Count"] ?? "").ToString());

                        if (dr["Printer Label Rev"].ToString() == "")
                        {
                            InputData.revisionedObjectField("PrinterLabelDefinition").setRef((dr["Printer Label Definition"] ?? "").ToString(), "", true);
                        }
                        else
                        {
                            InputData.revisionedObjectField("PrinterLabelDefinition").setRef((dr["Printer Label Definition"] ?? "").ToString(), (dr["Printer Label Rev"] ?? "").ToString(), true);
                        }
                        InputData.namedObjectField("PrintQueue").setRef((dr["Print Queue"] ?? "").ToString());
                        InputData.namedObjectField("TaskContainer").setRef(containerName);

                        // Service Excute and request Completion Msg
                        gService.setExecute();
                        gService.requestData().requestField("CompletionMsg");

                        // Print XMl Dcoument
                        PrintDoc(gDocument.asXML(), true);
                        ResponseDocument = gDocument.submit();
                        PrintDoc(ResponseDocument.asXML(), false);

                        ErrorsCheck(ResponseDocument);

                        dr.BeginEdit();
                        if (camMessage.Result)
                        {
                            successCnt++;
                        }

                        dr["Label Print Result"] = camMessage.Message;
                        dr["BoolResult"] = camMessage.Result;
                        dr.EndEdit();
                    }
                }
            }
            catch (Exception ex)
            {
                camMessage.Result = false;
                camMessage.Message = ex.Message;
            }

            DestroySession();

            return successCnt;
        }

        public bool ContainerAttribute(DataRow dr, List<string> attrList)
        {
            csiDocument ResponseDocument = null;
            csiObject InputData = null;
            csiSubentityList ServiceDetails = null;
            csiSubentity listItem = null;
            csiSubentity Attribute = null;
            csiParentInfo Parent = null;

            try
            {
                // Set Service Type
                CreateDocumentandService("ContainerAttrMaintDoc", "ContainerAttrMaint");

                // Set Input Data
                InputData = gService.inputData();
                InputData.namedObjectField("Container").setRef((dr["Container"] ?? "").ToString());

                ServiceDetails = InputData.subentityList("ServiceDetails");

                // Load Product Attribute
                query = string.Format("exec CAMDBsh.VE_VR_Proc_Common_ContainerAttribute '{0}'", (dr["Container"] ?? "").ToString());
                DataView camProductAttrView = db.GetDataView("productAttribute", query);

                foreach (DataRowView drv in camProductAttrView)
                {
                    listItem = ServiceDetails.appendItem();
                    Attribute = listItem.subentityField("Attribute");
                    Attribute.setObjectType("UserAttribute");
                    Parent = Attribute.parentInfo();
                    Parent.setContainerRef((dr["Container"] ?? "").ToString(), (dr["Level"] ?? "").ToString());
                    listItem.dataField("Name").setValue((drv["UserAttributeName"] ?? "").ToString());

                    if (attrList.Contains((drv["UserAttributeName"] ?? "").ToString()))
                    {
                        listItem.dataField("AttributeValue").setValue(dr[(drv["UserAttributeName"] ?? "").ToString()].ToString());
                    }
                    else
                    {
                        listItem.dataField("AttributeValue").setValue(drv["AttributeValue"].ToString());
                    }
                    listItem.dataField("DataType").setValue("4");
                    listItem.dataField("IsExpression").setValue("False");
                }

                InputData.namedObjectField("TaskContainer").setRef((dr["Container"] ?? "").ToString());

                // Service Excute and request Completion Msg
                gService.setExecute();
                gService.requestData().requestField("CompletionMsg");

                // Print XMl Dcoument
                PrintDoc(gDocument.asXML(), true);
                ResponseDocument = gDocument.submit();
                PrintDoc(ResponseDocument.asXML(), false);

                ErrorsCheck(ResponseDocument);

                dr.BeginEdit();
                dr["Attribute Result"] = camMessage.Message;
                dr["BoolResult"] = camMessage.Result;
                dr.EndEdit();
            }
            catch (Exception ex)
            {
                camMessage.Result = false;
                camMessage.Message = ex.Message;
            }

            return camMessage.Result;
        }

        #region 컨테이너 속성 변경 개발 중 22-11-17
        public int ContainerAttribute(DataTable table)
        {
            int successCnt = 0;

            csiDocument ResponseDocument = null;
            csiObject InputData = null;
            csiSubentityList ServiceDetails = null;
            csiSubentity listItem = null;
            csiSubentity Attribute = null;
            csiParentInfo Parent = null;

            try
            {
                CreateSession();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "세션 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            try
            {
                // Excel Template의 User Attribute 시작 Column의 조건을 걸어줌
                int attributeStartIndex = -1;

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    if (table.Columns[i].ColumnName == "Start Result")
                    {
                        attributeStartIndex = i + 1;
                        break;
                    }
                }

                foreach (DataRow dr in table.Rows)
                {
                    // Set Service Type
                    CreateDocumentandService("ContainerAttrMaintDoc", "ContainerAttrMaint");

                    // Set Input Data
                    InputData = gService.inputData();
                    InputData.namedObjectField("Container").setRef((dr["Container"] ?? "").ToString());

                    // Load Product Attribute
                    query = string.Format("exec CAMDBsh.VR_Proc_Common_ContainerAttribute '{0}'", (dr["Container"] ?? "").ToString());
                    DataView camProductAttrView = db.GetDataView("productAttribute", query);

                    foreach (DataRowView drv in camProductAttrView)
                    {
                        listItem = ServiceDetails.appendItem();
                        Attribute = listItem.subentityField("Attribute");
                        Attribute.setObjectType("UserAttribute");
                        Parent = Attribute.parentInfo();
                        Parent.setContainerRef((dr["Container"] ?? "").ToString(), (dr["Level"] ?? "").ToString());
                        listItem.dataField("Name").setValue((drv["UserAttributeName"] ?? "").ToString());

                        //for (int i = 0; i < length; i++)
                        //{


                        //}
                    }

                    // Excel Attribute Check

                    // Service Excute and request Completion Msg
                    gService.setExecute();
                    gService.requestData().requestField("CompletionMsg");

                    // Print XMl Dcoument
                    PrintDoc(gDocument.asXML(), true);
                    ResponseDocument = gDocument.submit();
                    PrintDoc(ResponseDocument.asXML(), false);

                    ErrorsCheck(ResponseDocument);

                    dr.BeginEdit();
                    if (camMessage.Result)
                    {
                        csiService csiservice = ResponseDocument.getService();
                        if (csiservice != null && (dr["Container"] ?? "").ToString() == "Auto")
                        {
                            csiDataField csidatafield = (csiDataField)csiservice.responseData().getResponseFieldByName("CompletionMsg");
                            dr["Container"] = csidatafield.getValue().Split(new char[] { ' ' })[0].Trim();
                        }
                        successCnt++;
                    }
                    else
                    {
                        dr["Container"] = "";
                    }

                    dr["Result"] = camMessage.Message;
                    dr["BoolResult"] = camMessage.Result;
                    dr.EndEdit();
                }
            }
            catch (Exception ex)
            {
                camMessage.Result = false;
                camMessage.Message = ex.Message;
            }

            DestroySession();

            return successCnt;
        }
        #endregion


        public CamstarMessage ContainerHoldLoop(string containerName, string holdReasonName)
        {
            csiDocument ResponseDocument = null;
            csiObject InputData = null;

            try
            {
                CreateDocumentandService("HoldDoc", "Hold");

                InputData = gService.inputData();

                InputData.namedObjectField("Container").setRef(containerName);
                InputData.namedObjectField("HoldReason").setRef(holdReasonName);

                gService.setExecute();
                gService.requestData().requestField("CompletionMsg");

                PrintDoc(gDocument.asXML(), true);
                ResponseDocument = gDocument.submit();
                PrintDoc(ResponseDocument.asXML(), false);

                ErrorsCheck(ResponseDocument);
            }
            catch (Exception ex)
            {
                camMessage.Result = false;
                camMessage.Message = ex.Message;
            }

            return camMessage;
        }

        public CamstarMessage ContainerReleaseLoop(string containerName, string releaseReasonName)
        {
            csiDocument ResponseDocument = null;
            csiObject InputData = null;

            try
            {
                CreateDocumentandService("ReleaseDoc", "Release");

                InputData = gService.inputData();

                InputData.namedObjectField("Container").setRef(containerName);
                InputData.namedObjectField("ReleaseReason").setRef(releaseReasonName);

                gService.setExecute();
                gService.requestData().requestField("CompletionMsg");

                PrintDoc(gDocument.asXML(), true);
                ResponseDocument = gDocument.submit();
                PrintDoc(ResponseDocument.asXML(), false);

                ErrorsCheck(ResponseDocument);
            }
            catch (Exception ex)
            {
                camMessage.Result = false;
                camMessage.Message = ex.Message;
            }

            return camMessage;
        }

        #endregion

        #region WonKeun,Cho test 2022-12-15

        public int ResourceSetup(DataTable table)
        {
            int successCnt = 0;
            csiDocument ResponseDocument = null;
            csiObject InputData = null;
            csiSubentity Details = null;

            try
            {
                CreateSession();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "세션 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            try
            {
                foreach (DataRow dr in table.Rows)
                {
                    // Set Service Type
                    CreateDocumentandService("ResourceSetupTransitionTrans", "ResourceSetupTransition");

                    // Set Input Data
                    InputData = gService.inputData();

                    // Set Resource Element
                    InputData.dataField("Availability").setValue("1");
                    InputData.namedObjectField("Resource").setRef((dr["Resource"] ?? "").ToString());
                    InputData.namedObjectField("ResourceStatusCode").setRef((dr["Resource StatusCode"] ?? "").ToString());

                    // Service Excute and request Completion Msg
                    gService.setExecute();
                    gService.requestData().requestField("CompletionMsg");
                    gService.requestData().requestField("ACEMessage");
                    gService.requestData().requestField("ACEStatus");

                    // Print XMl Dcoument
                    PrintDoc(gDocument.asXML(), true);
                    ResponseDocument = gDocument.submit();
                    PrintDoc(ResponseDocument.asXML(), false);

                    ErrorsCheck(ResponseDocument);

                    dr.BeginEdit();
                    
                    dr["Result"] = camMessage.Message;
                    dr["BoolResult"] = camMessage.Result;
                    dr.EndEdit();

                    if (camMessage.Result)
                    {
                        successCnt++;
                    }
                }
            }
            catch (Exception ex)
            {
                camMessage.Result = false;
                camMessage.Message = ex.Message;
            }

            DestroySession();

            return successCnt;
        }

        #endregion

    }
}
