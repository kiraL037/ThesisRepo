using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.UI.ViewModels;
using ThesisProjectARM.UI.Views.Windows;
using UI.ViewModels;

namespace UI
{
    public class ViewModelLocator
    {
        public AuthenticationWindowVM AuthenticationWindowVM => App._container.GetInstance<AuthenticationWindowVM>();
        public RegistrationVM RegistrationVM => App._container.GetInstance<RegistrationVM>();
        public AdminWindowVM AdminWindowVM => App._container.GetInstance<AdminWindowVM>();
        public AnalysisVM AnalysisVM => App._container.GetInstance<AnalysisVM>();
        public DataPageVM DataPageVM => App._container.GetInstance<DataPageVM>();
        public DataVisualizationVM DataVisualizationVM => App._container.GetInstance<DataVisualizationVM>();
        public DBCRUDVM DBCRUDVM => App._container.GetInstance<DBCRUDVM>();
        public FirstSetupWindowVM FirstSetupWindowVM=> App._container.GetInstance<FirstSetupWindowVM>();
        public MainUIVM MainUIVM => App._container.GetInstance<MainUIVM>();
        public ManagerVM ManagerVM => App._container.GetInstance<ManagerVM>();
        public SelectTableVM SelectTableVM => App._container.GetInstance<SelectTableVM>();
        public TipsWindowVM TipsVM => App._container.GetInstance<TipsWindowVM>();
        public WelcomeWindowVM WelcomeVM => App._container.GetInstance<WelcomeWindowVM>();
        public ViewModelBase ViewModelBase => App._container.GetInstance<ViewModelBase>();
        public RelayCommand RelayCommand => App._container.GetInstance<RelayCommand>();
        public DataExportVM DataExportVM => App._container.GetInstance<DataExportVM>();
    }
}
