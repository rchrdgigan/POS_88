using System;
using System.IO;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using POS_88.Database;
using POS_88.Persistence;
using POS_88.Report;
using POS_88.Views;

namespace POS_88.Presenters
{
    public class ReportViewerPresenter
    {
        private readonly IReportViewerView _view;

        public ReportViewerPresenter(IReportViewerView view)
        {
            _view = view;
        }

        public void LoadReport()
        {
            ReportDocument reportDocument = null;
            UnitOfWorkPOS unitOfWork = new UnitOfWorkPOS(new POSDBEntities());

            try
            {
                setting settings = unitOfWork.Setting
                    .Find(item => item.settingsid == 1)
                    .FirstOrDefault();
                if (settings == null)
                    throw new InvalidOperationException("Database settings were not found.");

                string reportTypeName = Path.GetFileNameWithoutExtension(_view.ReportName);
                Type reportType = typeof(Inventory).Assembly.GetType(
                    "POS_88.Report." + reportTypeName,
                    false,
                    false);
                if (reportType == null || !typeof(ReportDocument).IsAssignableFrom(reportType))
                    throw new InvalidOperationException("The report could not be found: " + _view.ReportName);

                reportDocument = (ReportDocument)Activator.CreateInstance(reportType);

                ConnectionInfo connectionInfo = new ConnectionInfo
                {
                    ServerName = settings.ServerNm,
                    DatabaseName = settings.DatabaseNm,
                    UserID = settings.DbUser,
                    Password = settings.DbPw
                };

                foreach (Table table in reportDocument.Database.Tables)
                {
                    TableLogOnInfo logOnInfo = table.LogOnInfo;
                    logOnInfo.ConnectionInfo = connectionInfo;
                    table.ApplyLogOnInfo(logOnInfo);
                }

                if (_view.ParameterNames == null || _view.ParameterValues == null ||
                    _view.ParameterNames.Count != _view.ParameterValues.Count)
                    throw new InvalidOperationException("The report parameters are invalid.");

                for (int index = 0; index < _view.ParameterNames.Count; index++)
                {
                    string parameterName = _view.ParameterNames[index].ToString();
                    reportDocument.SetParameterValue(parameterName, _view.ParameterValues[index]);
                }

                _view.ShowReport(reportDocument);
                reportDocument = null;
            }
            finally
            {
                unitOfWork.Dispose();
                if (reportDocument != null)
                {
                    reportDocument.Close();
                    reportDocument.Dispose();
                }
            }
        }
    }
}