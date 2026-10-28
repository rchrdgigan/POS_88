using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_88.Views
{
    public partial class ReportViewerView : Form, IReportViewerView
    {
        private string _RptNm = "";
        private ArrayList _paramArrayList;
        private ArrayList _paramValueArrayList;

        public string ReportName => _RptNm;
        public ArrayList ParameterNames => _paramArrayList;
        public ArrayList ParameterValues => _paramValueArrayList;

        public ReportViewerView(
            string rptNm,
            ArrayList paramArrayList,
            ArrayList paramValueArrayList)
        {
            InitializeComponent();

            _RptNm = rptNm;
            _paramArrayList = paramArrayList;
            _paramValueArrayList = paramValueArrayList;
            FormClosed += (sender, args) =>
            {
                ReportDocument reportDocument = myCrystalReportViewer.ReportSource as ReportDocument;
                myCrystalReportViewer.ReportSource = null;
                if (reportDocument != null)
                {
                    reportDocument.Close();
                    reportDocument.Dispose();
                }
            };
        }

        public void ShowReport(object report)
        {
            myCrystalReportViewer.ReportSource = report;
        }
    }
}
