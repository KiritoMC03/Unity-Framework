using System.Collections;
using System.Collections.Generic;
using General.Mediator;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Mediator.Test
{
    public class Observer : MonoBehaviour
    {
        [UnityTest, Order(1)]
        public IEnumerator RemoveObserverOnChangeComponent()
        {
            bool testResult = default;
            
            var testObserver = new ObserverSingleComponent<SingleComponentTest>(this);
            var testObserver1 = new ObserverSingleComponent<SingleComponentTest>(this);
            var obj = new SingleComponentTest();
            testObserver.ChangeComponent += test =>
            {
                Debug.Log("Remove");
                testObserver.Release();
            };
            testObserver1.ChangeComponent += test =>
            {
                Debug.Log("Okay");
            };
            
            MC.Instance.Add(obj);

            yield return new WaitUntil(Check);

            MC.Init(new MediatorSystem());
            
            if (testResult)
            {
                Assert.Pass(nameof(RemoveObserverOnChangeComponent));
            }
            else
            {
                Assert.Fail(nameof(RemoveObserverOnChangeComponent));
            }
            
            bool Check()
            {
                var mediator = (MediatorSystem) MC.Instance;
                var list = mediator.ObserversSystem.GetObservers<SingleComponentTest,ObserverSingleComponent<SingleComponentTest>>(obj);
                return testResult = list.Count == 1;
            }
        }
    }
    
    class SingleComponentTest : ISingleComponent{}
}
