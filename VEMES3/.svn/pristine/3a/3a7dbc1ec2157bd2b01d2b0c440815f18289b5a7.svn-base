using DevExpress.DataAccess.Wizard.Presenters;
using DevExpress.Drawing;
using DevExpress.Spreadsheet;
using DevExpress.Spreadsheet.Export;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Export;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VTMES3_RE.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace VTMES3_RE.View.CamstarInf
{
    public partial class frmPrintContainerLabel : DevExpress.XtraEditors.XtraForm
    {
        string folderName = Application.StartupPath + @"\Templates\LabelPrint";
        string fileName = "";
        bool IsSubmit = false;

        public frmPrintContainerLabel()
        {
            InitializeComponent();

            if (WrGlobal.Camster_Common == null)
            {
                WrGlobal.Camster_Common = new CamstarCommon();
            }

            DirectoryInfo di = new DirectoryInfo(folderName);
            int FileIdx = 0;

            foreach (System.IO.FileInfo file in di.GetFiles())
            {
                FileIdx++;

                TileNavCategory categoryItem = new TileNavCategory();
                categoryItem.Appearance.Options.UseFont = true;
                categoryItem.AppearanceHovered.Options.UseFont = true;
                categoryItem.AppearanceSelected.Options.UseFont = true;
                categoryItem.Name = "categoryItem" + FileIdx.ToString();
                categoryItem.Caption = file.Name;
                categoryItem.TileText = file.Name;
                categoryItem.ElementClick += new NavElementClickEventHandler(this.categoryItem_ElementClick);
                tileNavPane.Categories.Add(categoryItem);

                //if (categoryItem.Tag.ToString() == "defaultfilename")
                //{
                //    tileNavPane.SelectedElement = categoryItem;
                //}
            }
        }

        private void categoryItem_ElementClick(object sender, NavElementEventArgs e)
        {
            fileName = ((DevExpress.XtraBars.Navigation.TileNavElement)e.Element).TileText;
            excelSheetControl.LoadDocument(folderName + "\\" + fileName);
            IsSubmit = false;
        }

        private void exporter_CellValueConversionError(object sender, CellValueConversionErrorEventArgs e)
        {
            DataTableExporter exporter = sender as DataTableExporter;
            CellValueToColumnTypeConverter defaultToColumnTypeConverter = exporter != null ? exporter.Options.DefaultCellValueToColumnTypeConverter : null;
            if (e.DataColumn.DataType == typeof(Double) && e.CellValue.IsText)
            {
                object newDataTableValue = CellValue.Empty;
                ConversionResult isConverted = defaultToColumnTypeConverter.Convert(e.Cell, e.CellValue, e.DataColumn.DataType, out newDataTableValue);
                e.DataTableValue = newDataTableValue;
                e.Action = isConverted == ConversionResult.Success ? DataTableExporterAction.Continue : DataTableExporterAction.SkipRow;
            }
        }

        //private void tileNavPane_SelectedElementChanged(object sender, TileNavElementEventArgs e)
        //{
        //    fileName = e.Element.TileText;
        //    excelSheetControl.LoadDocument(folderName + "\\" + fileName);
        //}

        private void cmdExecute_ElementClick(object sender, NavElementEventArgs e)
        {
            if (fileName == "") return;

            if (IsSubmit)
            {
                MessageBox.Show("현재 Camstar Interface 기능이 실행 중 입니다.\n잠시 후 다시 제출하세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            WrGlobal.Camster_Common.IsExecuting = true;
            IsSubmit = true;

            try
            {
                Worksheet worksheet = excelSheetControl.Document.Worksheets[0];

                CellRange range = worksheet.GetDataRange();

                DataTable dataTable = worksheet.CreateDataTable(range, true);
                dataTable.TableName = "ExcelUpload";

                DataTableExporter exporter = worksheet.CreateDataTableExporter(range, dataTable, true);
                exporter.Options.ConvertEmptyCells = true;
                exporter.Options.DefaultCellValueToColumnTypeConverter.EmptyCellValue = 0;
                exporter.Options.DefaultCellValueToColumnTypeConverter.SkipErrorValues = true;

                exporter.CellValueConversionError += exporter_CellValueConversionError;
                exporter.Export();

                if (dataTable.Rows.Count < 1)
                {
                    WrGlobal.Camster_Common.IsExecuting = false;
                    MessageBox.Show("제출할 항목이 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                foreach (DataRow dr in dataTable.Rows)
                {
                    if ((dr["Container"] ?? "").ToString() == "")
                    {
                        WrGlobal.Camster_Common.IsExecuting = false;
                        MessageBox.Show("Container는 필수 입력 항목입니다. .", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                }

                dataTable.Columns.Add("BoolResult", typeof(System.Boolean));

                lblMemo.Text += "전체 : " + dataTable.Rows.Count.ToString() + "건 | ";

                int successCnt = 0;

                successCnt = WrGlobal.Camster_Common.PrintContainerLabel(dataTable);

                if (successCnt == -1)
                {
                    WrGlobal.Camster_Common.IsExecuting = false;
                    return;
                }

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        switch (dataTable.Columns[j].ColumnName)
                        {
                            case "Container":
                            case "Result":
                                worksheet.Cells[range.TopRowIndex + 1 + i, range.LeftColumnIndex + j].SetValue(dataTable.Rows[i][j]);
                                break;
                            case "BoolResult":
                                if (!Convert.ToBoolean(dataTable.Rows[i][j]))
                                {
                                    for (int k = 0; k < range.ColumnCount; k++)
                                    {
                                        range[i + 1, k].FillColor = Color.Red;
                                    }
                                }
                                break;
                        }
                    }
                }
                lblMemo.Text += "성공 : " + successCnt.ToString() + "건 | ";
                lblMemo.Text += "실패 : " + (dataTable.Rows.Count - successCnt).ToString() + "건 | ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            WrGlobal.Camster_Common.IsExecuting = false;
        }

        private void cmdClose_ElementClick(object sender, NavElementEventArgs e)
        {
            this.Close();
        }
    }
}