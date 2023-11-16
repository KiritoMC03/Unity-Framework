using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using General.Extensions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using General.Mediator;

namespace Mediator.Test
{
    public class Mediator
    {
        #region SingleComponent
        
        public class SingleComponent
        {
            
            [Test]
            public void AddSingleComponentForGet()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new SingleComponentTest();
                mediator.Add(obj);
                mediator.GetSingleComponent(this, out SingleComponentTest test);
                Assert.AreSame(test, obj);
            }
            
            [Test]
            public void AddSingleComponentWithInterfaceForGetByInterface()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new SingleComponentWithInterfaceTest();
                mediator.Add<ISingleComponentInterfaceTest>(obj);
                mediator.GetSingleComponent(this, out ISingleComponentInterfaceTest test);
                Assert.AreSame(test, obj);
            }
            
            [Test]
            public void ReplaceSingleComponentWithInterfaceForGet()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new SingleComponentWithInterfaceTest();
                mediator.Add<ISingleComponentInterfaceTest>(obj);
                mediator.GetSingleComponent(this, out ISingleComponentInterfaceTest test);
                Assert.AreSame(test, obj);

                var obj2 = new SingleComponentWithInterfaceTest2();
                mediator.Add<ISingleComponentInterfaceTest>(obj2, SetMode.Force);
                mediator.GetSingleComponent(this, out ISingleComponentInterfaceTest test2);
                Assert.AreSame(test2, obj2);
            }
            
            [Test]
            public void AddSingleComponentRemoveAddAgain()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new SingleComponentTest();
                mediator.Add(obj);
                var obj2 = new SingleComponentTest();
                mediator.Add(obj2, SetMode.Force);
                mediator.GetSingleComponent(this, out SingleComponentTest test);
                Assert.AreSame(test, obj2);
            }

            [Test]
            public void AddSingleComponentWitPermissionForGet()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new SingleComponentTest();
                mediator.Add(obj,SetMode.None ,typeof(SingleComponent));
                mediator.GetSingleComponent(this, out SingleComponentTest test);
                Assert.AreSame(test, obj);
            }

            [Test]
            public void AddSingleComponentWithWrongPermissionForGet()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new SingleComponentTest();
                mediator.Add(obj, SetMode.None,typeof(SingleComponentTest));
                mediator.GetSingleComponent(this, out SingleComponentTest test);
                Assert.IsNull(test);
            }

            [Test]
            public void AddSingleComponentWithLockForGet()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new SingleComponentTest();
                mediator.Add(obj);
                mediator.SetLock(typeof(SingleComponentTest), true);
                mediator.GetSingleComponent(this, out SingleComponentTest test);
                Assert.IsNull(test);
            }

            [Test]
            public void AddSingleComponentForGetAcync()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new SingleComponentTest();
                mediator.GetSingleComponentsAsync<SingleComponent, SingleComponentTest>(this, SetValue);

                void SetValue(SingleComponentTest componentTest)
                {
                    Assert.Equals(componentTest, obj);
                }

                mediator.Add(obj);
            }
            
            [Test]
            public void ReplaceSingleComponentWithAnotherType()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new SingleComponentWithInterfaceTest();
                var go = new GameObject(nameof(ReplaceSingleComponentWithAnotherType));
                var mono = go.AddComponent<SingleComponentMonoBehaviourWithInterface>();
        
                mediator.Add<ISingleComponentInterfaceTest>(obj);
                mediator.GetSingleComponent(this, out ISingleComponentInterfaceTest test);
                mediator.Add<ISingleComponentInterfaceTest>(mono,SetMode.Force);
                mediator.GetSingleComponent(this, out ISingleComponentInterfaceTest test1);
                mediator.Add<ISingleComponentInterfaceTest>(obj);
                mediator.Add<ISingleComponentInterfaceTest>(obj,SetMode.Force);
                mediator.GetSingleComponent(this, out ISingleComponentInterfaceTest test2);
                
                UnityEngine.Object.DestroyImmediate(go);
                Assert.True(test == test2 && test1 != test);
            }
        }

        #endregion
        
        #region WeakSingleComponent
        
        public class WeakSingleComponent
        {
            
            [Test]
            public void AddWeakSingleComponentForGet()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new WeakSingleComponentTest();
                mediator.Add(obj);
                mediator.GetWeakSingleComponent(this, out WeakSingleComponentTest test);
                Assert.AreSame(test, obj);
            }
            
            [Test]
            public async void AddWeakSingleComponentForGetNull()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new WeakSingleComponentTest();
                var weak = new WeakReference<WeakSingleComponentTest>(obj);
                mediator.Add(obj);
                obj = null;
                await Task.Yield();
                GC.Collect();
                await Task.Yield();
                obj?.ToString();
                mediator.GetWeakSingleComponent(this, out WeakSingleComponentTest test);
                if(test is null) 
                    Assert.Pass();
                else
                    Assert.Fail();
            }
            
            [Test]
            public void AddWeakSingleComponentWithInterfaceForGetByInterface()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new WeakSingleComponentWithInterfaceTest();
                mediator.Add<IWeakSingleComponentInterfaceTest>(obj);
                mediator.GetWeakSingleComponent(this, out IWeakSingleComponentInterfaceTest test);
                Assert.AreSame(test, obj);
            }
            
            [Test]
            public void ReplaceWeakSingleComponentWithInterfaceForGet()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new WeakSingleComponentWithInterfaceTest();
                mediator.Add<IWeakSingleComponentInterfaceTest>(obj);
                var obj2 = new WeakSingleComponentWithInterfaceTest2();
                mediator.Add<IWeakSingleComponentInterfaceTest>(obj2, SetMode.Force);
                mediator.GetWeakSingleComponent(this, out IWeakSingleComponentInterfaceTest test);
                test.ToString();
                Assert.AreSame(test, obj2);
            }
            
            [Test]
            public void AddWeakSingleComponentRemoveAddAgain()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new WeakSingleComponentTest();
                mediator.Add(obj);
                var obj2 = new WeakSingleComponentTest();
                mediator.Add(obj2, SetMode.Force);
                mediator.GetWeakSingleComponent(this, out WeakSingleComponentTest test);
                Assert.AreSame(test, obj2);
            }

            [Test]
            public void AddWeakSingleComponentWitPermissionForGet()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new WeakSingleComponentTest();
                mediator.Add(obj,SetMode.None ,typeof(WeakSingleComponent));
                mediator.GetWeakSingleComponent(this, out WeakSingleComponentTest test);
                Assert.AreSame(test, obj);
            }

            [Test]
            public void AddWeakSingleComponentWithWrongPermissionForGet()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new WeakSingleComponentTest();
                mediator.Add(obj, SetMode.None,typeof(SingleComponentTest));
                mediator.GetWeakSingleComponent(this, out WeakSingleComponentTest test);
                Assert.IsNull(test);
            }

            [Test]
            public void AddWeakSingleComponentWithLockForGet()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new WeakSingleComponentTest();
                mediator.Add(obj);
                mediator.SetLock(typeof(WeakSingleComponentTest), true);
                mediator.GetWeakSingleComponent(this, out WeakSingleComponentTest test);
                Assert.IsNull(test);
            }

            [Test]
            public void ReplaceWeakSingleComponentWithAnotherType()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new WeakSingleComponentWithInterfaceTest();
                var go = new GameObject(nameof(ReplaceWeakSingleComponentWithAnotherType));
                var mono = go.AddComponent<WeakSingleComponentMonoBehaviourWithInterface>();
        
                mediator.Add<IWeakSingleComponentInterfaceTest>(obj);
                mediator.GetWeakSingleComponent(this, out IWeakSingleComponentInterfaceTest test);
                mediator.Add<IWeakSingleComponentInterfaceTest>(mono,SetMode.Force);
                mediator.GetWeakSingleComponent(this, out IWeakSingleComponentInterfaceTest test1);
                mediator.Add<IWeakSingleComponentInterfaceTest>(obj);
                mediator.Add<IWeakSingleComponentInterfaceTest>(obj,SetMode.Force);
                mediator.GetWeakSingleComponent(this, out IWeakSingleComponentInterfaceTest test2);
                
                UnityEngine.Object.DestroyImmediate(go);
                Assert.True(test == test2 && test1 != test);
            }
        }

        #endregion
        
        #region ObserverSingleComponent
        
        public class ObserverSingleComponent
        {
            [Test]
            public void CheckingForValueFromObserverAfterAdd()
            {
                IMediator mediator = new MediatorSystem();
                MC.Init(mediator);
                var testObserver = new ObserverSingleComponent<SingleComponentTest>(this);
                var obj = new SingleComponentTest();
                mediator.Add(obj);
                var obj1 = testObserver.Result;
                Assert.AreSame(obj1, obj);
            }
            
            [Test]
            public void CheckingForValueFromObserverBeforeAdd()
            {
                IMediator mediator = new MediatorSystem();
                MC.Init(mediator);
                var obj = new SingleComponentTest();
                mediator.Add(obj);
                var testObserver = new ObserverSingleComponent<SingleComponentTest>(this);
                var obj1 = testObserver.Result;
                Assert.AreSame(obj1, obj);
            }
            
            [Test]
            public void CheckingAccessWithPermissions()
            {
                IMediator mediator = new MediatorSystem();
                MC.Init(mediator);
                var obj = new SingleComponentTest();
                mediator.Add(obj, SetMode.None, typeof(ObserverSingleComponent));
                var testObserver = new ObserverSingleComponent<SingleComponentTest>(this);
                var obj1 = testObserver.Result;
                Assert.AreSame(obj1, obj);
            }
            
            [Test]
            public void CheckingNotAccessWithPermissions()
            {
                IMediator mediator = new MediatorSystem();
                MC.Init(mediator);
                var obj = new SingleComponentTest();
                mediator.Add(obj, SetMode.None, typeof(SingleComponentTest));
                var testObserver = new ObserverSingleComponent<SingleComponentTest>(this);
                var obj1 = testObserver.Result;
                Assert.IsTrue(obj1.IsNull() && !obj1.Equals(obj));
            }
        }

        #endregion

        #region Components
        
        public class Components
        {
            [Test]
            public void AddComponentsForGet()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new ComponentsTest();
                mediator.Add(obj);
                mediator.GetComponents(this, out List<ComponentsTest> test);
                Assert.AreSame(test[0], obj);
            }
            
            [Test]
            public void AddComponentsWithInterfaceForGet()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new ComponentsWithInterfaceTest();
                mediator.Add<IComponentsInterfaceTest>(obj);
                mediator.GetComponents(this, out List<IComponentsInterfaceTest> test);
                Assert.AreSame(test[0], obj);
            }
            
            [Test]
            public void AddSomeComponentsWithInterfaceForGet()
            {
                int objNumber = 11;
                IMediator mediator = new MediatorSystem();
                var startedList = new IComponentsInterfaceTest[objNumber];
                for (int i = 0; i < objNumber; i++)
                {
                    var obj = new ComponentsWithInterfaceTest();
                    mediator.Add<IComponentsInterfaceTest>(obj);
                    startedList[i] = obj;
                }
                mediator.GetComponents(this, out List<IComponentsInterfaceTest> test);
                Assert.AreEqual(test.Count, objNumber);
                for (int i = 0; i < objNumber; i++) Assert.IsTrue(test.Contains(startedList[i]));
            }

            [Test]
            public void AddComponentsWitPermissionForGet()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new ComponentsTest();
                mediator.Add(obj, SetMode.None,typeof(Components));
                mediator.GetComponents(this, out List<ComponentsTest> test);
                Assert.AreSame(test[0], obj);
            }

            [Test]
            public void AddComponentsWithWrongPermissionForGet()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new ComponentsTest();
                mediator.Add(obj, SetMode.None,typeof(SingleComponentTest));
                mediator.GetComponents(this, out List<ComponentsTest> test);
                Assert.IsNull(test);
            }

            [Test]
            public void AddComponentsWithLockForGet()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new ComponentsTest();
                mediator.Add(obj);
                mediator.SetLock(typeof(ComponentsTest), true);
                mediator.GetComponents(this, out List<ComponentsTest> test);
                Assert.IsNull(test);
            }

            [Test]
            public void AddComponentsForGetAcync()
            {
                IMediator mediator = new MediatorSystem();
                var obj = new ComponentsTest();
                mediator.GetComponentsAsync<Components, ComponentsTest>(this, SetValue);

                void SetValue(List<ComponentsTest> test)
                {
                    Assert.Equals(test[0], obj);
                }

                mediator.Add(obj);
                
            }
        }
        
        
        #endregion
    }
}

internal class SingleComponentTest : ISingleComponent{}

internal class WeakSingleComponentTest : IWeakSingleComponent{}

internal class SingleComponentWithInterfaceTest : ISingleComponentInterfaceTest { }

internal class SingleComponentWithInterfaceTest2 : ISingleComponentInterfaceTest { }

internal class SingleComponentMonoBehaviourWithInterface : MonoBehaviour, ISingleComponentInterfaceTest { }

internal class WeakSingleComponentWithInterfaceTest : IWeakSingleComponentInterfaceTest{ }

internal class WeakSingleComponentWithInterfaceTest2 : IWeakSingleComponentInterfaceTest { }

internal class WeakSingleComponentMonoBehaviourWithInterface : MonoBehaviour, IWeakSingleComponentInterfaceTest { }

internal class ComponentsTest : IComponents{}

internal class ComponentsWithInterfaceTest : IComponentsInterfaceTest { }

internal interface ISingleComponentInterfaceTest : ISingleComponent { }

internal interface IWeakSingleComponentInterfaceTest : IWeakSingleComponent { }

internal interface IComponentsInterfaceTest : IComponents { }
