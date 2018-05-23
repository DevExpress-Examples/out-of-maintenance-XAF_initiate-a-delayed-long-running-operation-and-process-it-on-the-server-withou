using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using LongRunningOperations.Module.BusinessObjects;

namespace LongRunningOperations.Module.Controllers {
    public class StartProcessingController : ViewController {
        public StartProcessingController() {
            SimpleAction action = new SimpleAction(this, "StartProcessing", PredefinedCategory.Edit);
            action.Execute += new SimpleActionExecuteEventHandler(action_Execute);
        }
        void action_Execute(object sender, SimpleActionExecuteEventArgs e) {
            StartProcessing();
        }
        public void StartProcessing() {
            IObjectSpace os = Application.CreateObjectSpace();
            ObjectForLongRunningOperations obj = os.CreateObject<ObjectForLongRunningOperations>();
            obj.ObjectToProcess = (Object1)os.GetObject(View.CurrentObject);
            if(obj.ObjectToProcess == null) {
                throw new ArgumentException();
            }
            os.CommitChanges();
            ((Object1)View.CurrentObject).SetIsInProcessing(true);
            Frame.GetController<AppearanceController>().Refresh();
        }
    }
}
