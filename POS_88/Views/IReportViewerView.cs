using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;


namespace POS_88.Views
{
    public interface IReportViewerView
    {
        string ReportName { get; }
        ArrayList ParameterNames { get; }
        ArrayList ParameterValues { get; }

        void ShowReport(object report);
    }

}
