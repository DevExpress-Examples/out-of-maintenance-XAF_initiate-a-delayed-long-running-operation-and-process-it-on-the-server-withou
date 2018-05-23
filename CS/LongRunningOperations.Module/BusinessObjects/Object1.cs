using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace LongRunningOperations.Module.BusinessObjects {
    [DefaultClassOptions]
    [Appearance("IsInProcessing", "[IsInProcessing]='True'", Enabled = false, TargetItems="*")]
    public class Object1 : BaseObject {
        internal bool? isInProcessing; 
        private string name;
        private string description;
        public string Name {
            get { return name; }
            set {
                SetPropertyValue<string>("Name", ref name, value);
            }
        }
        [Size(SizeAttribute.Unlimited)]
        public string Description {
            get { return description; }
            set {
                SetPropertyValue<string>("Description", ref description, value);
            }
        }
        public bool IsInProcessing {
            get {
                if(!isInProcessing.HasValue) {
                    isInProcessing = (Session.FindObject<ObjectForLongRunningOperations>(new BinaryOperator("ObjectToProcess", this)) != null);
                }
                return isInProcessing.Value; 
            }
        }
        public Object1(Session session)
            : base(session) {

        }
        public void SetIsInProcessing(bool val) {
            isInProcessing = val;
            RaisePropertyChangedEvent("IsInProcessing");
        }
    }
}