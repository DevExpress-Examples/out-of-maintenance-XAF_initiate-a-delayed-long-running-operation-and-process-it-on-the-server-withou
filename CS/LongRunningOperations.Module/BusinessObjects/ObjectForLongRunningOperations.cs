using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;

namespace LongRunningOperations.Module.BusinessObjects {
    [DefaultClassOptions]
    public class ObjectForLongRunningOperations : BaseObject{
        public ObjectForLongRunningOperations(Session session)
            : base(session) {
        }
        private Object1 obj;
        public Object1 ObjectToProcess {
            get { return obj; }
            set { SetPropertyValue<Object1>("ObejectToProcess", ref obj, value); }
        }
    }
}
