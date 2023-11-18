# Framework.Base

## [1.3.0] - 2023-11-18
* `Massive rework`
* Renamed to Framework.Base.
* Added IndexationRegistry service as new variant of Dependency-injection-system.
* InterfaceItem<> and InterfacesList<> structures for pseudo 'Interfaces serialization'.
* Added other extended collections: serialized/observable Dictionary/HashSet, etc.
* Added StringBasedIdentifier for serialize string and show it in Unity inspector.
* Added Application Settings module.
* Added Timer module.
* Added CollisionListener and TriggerListener.

## [1.2.2] - 2022-12-15
* `Mediator & Collections`
* The ObserverSingleComponent<T> will require an appealType in the constructor to set access to the mediator.
* Added ObservableObjectLite<T>.

## [1.2.1] - 2022-12-14
* `Object pooler & Extensions & Serialization`
* Refactored ObjectPooler.
* Fixed bug with non DontDestroyOnLoad pools in ObjectPooler.
* Added the ability to set an associated Int for enum PooledObjectType (ObjectPooler).
* Added Pool<T> for non UnityEngine.Object inheritors.
* Refactor object.IsNull() & object.NotNull() extensions methods.
* Added CSharpInterfaceItemDrawer for easy selecting interface implementation in inspector.

## [1.2.0] - 2022-10-14
* `CSV`
* Added CSV automation.

## [1.1.1] - 2022-10-14
* `Mediator`
* Fixed compilation error related to `IL2CPP`.

## [1.1.0] - 2022-10-13
* `Mediator`
* Fully encapsulated registration of `ObserverSingleComponent` and remove subscription on `Mediator`.
* Added the ability to remove the subscription to `ObserverSingleComponent` at runtime in the `ChangeComponent` event.
* Added new tests in `PlayMode` to test the `ObserverSingleComponent` component.
* Added all the main methods to the top-level top-level `Mediator` wrapper.
* Created a way to integrate custom `Mediators` into the top-level `Mediator` wrapper.
* Fixed namespace for `Stack trace`.
* Template has been fixed to create an auto-registered script.
* Added dependency on Continuous integration module version `0.0.6`.

## [1.0.2] - 2022-10-11
* `Object Pooler`
* Extracted IObjectPooler interface.

## [1.0.1] - 2022-10-04
* `Object Pooler & Extensions`
* Added scroll view to object pooler window.
* Added string extensions.

## [1.0.0] - 2022-09-21
* `Mediator`
* Added new interface `IWeakSingalComonent`, that allows you to organize weak references between components.
* Added new component `ObserverSingleComponent`, that allows you to observe all changes within the mediator for `IWeakSingleComponent`, `ISingleComponent` interfaces.
* Refactoring was done related to the addition of new functionality.

## [0.7.1] - 2022-09-06
* `DM`
* Added the ability to change the transparency of the debug menu button within runtime.

## [0.7.0] - 2022-08-10
* `Object Pooler & Extensions`
* DontDestroyOnLoad pools and memory fragmentation protection in pooler.
* Migrate unity-specific extensions to game-kit.

## [0.6.14] - 2022-08-01
* `Extensions`
* Added StackExtensions and QueueExtensions.

## [0.6.13] - 2022-07-29
* `General`
* IL2CPP configurator was moved from General plugin to Continuous integration plugin.

## [0.6.12] - 2022-07-20
* `Extensions & Serialization`
* Added InterfaceItem<T> - performs InterfaceCheckerAttribute duties and gives access to an interface (T) or UnityEngine.Component.
* Added TypeExtensions, Int32Extensions, HashSetExtensions, GenericExtensions and add methods to other extension class.
* Refactor extension methods.

## [0.6.11] - 2022-07-14
* `Mediator`
* Fixed ComponentsList refresh in force mode.

## [0.6.10] - 2022-07-14
* `Mediator`
* Fixed replace type for same interface in force mode.

## [0.6.9] - 2022-07-08
* `General`
* Fixed il2cpp compiler configurator functionality, in particular, 
*   before building, the compiler is configured, 
*   for the correct operation of the General plugin (Only for `Unity 2021.3..`).

## [0.6.8] - 2022-06-02
* `General`
* Added il2cpp compiler configurator functionality, in particular, 
*   before building, the compiler is configured, 
*   for the correct operation of the General plugin (Only for `Unity 2021.3..`).

## [0.6.7] - 2022-05-31
* `Serialization`
* Added SerializedInterfacesList, ObservableClass and ObservableData(struct).
* Added IListExtension (includes ObservableCollectionExtension and ListExtension).

## [0.6.6] - 2022-05-30
* `Mediator`
* Added an object to the mediator via the interface has been fixed.
* Added tests for adding an object to the mediator by interface.

## [0.6.5] - 2022-05-16]
* `Extension`
* Added Extensions for ObservableCollection.
* Fix methods GetRandomItem() in ArrayExtensions and ListExtensions.

## [0.6.4] - 2022-05-11]
* `Mediator`
* Corrected the name of the methods.

## [0.6.3] - 2022-04-19]
* `Extension`
* Added MonoBehaviourExtensions with InvokeDelayed() and DictionaryExtension with AddIfNotContains().

## [0.6.2] - 2022-04-19]
* `Serialization`
* Added SerializedHashSet<T> with all HashSet constructors, properties and methods.

## [0.6.1] - 2022-04-14]
* `DependencyController`
* Fixed call method resolving dependencies in `Mono` mode.

## [0.6.0] - 2022-04-07]
* `Mediator`
* Added the ability to visualize the added objects in the mediator, as well as the ability to get a Stack Trace.
* `DependencyController`
* The path to use the `DependencyController` in the editor has been changed and moved to `General plugin`
* Fixed recursive call method resolving dependencies.
* `DM`
* The path to use the `DM` in the editor has been changed and moved to `General plugin`.
* Disabled sending warning in build. 

## [0.5.11] - 2022-03-30]
* `DM`
* Fixed placement of debug menu buttons.

## [0.5.10] - 2022-03-23]
* `Serialization`
* Added constructors for SerializedObservableCollection and SerializedDictionary.
* SerializedDictionary key can be class.

## [0.5.9] - 2022-03-22
* `Helper`
* Added helper classes `CameraFrustumVisualizer`,`SkinnedMeshRendererBoundVisualizer`.

## [0.5.8] - 2022-03-18
* `DM`
* The work of the debug menu is automated to work only as part of the IL2CPP configuration 

## [0.5.7] - 2022-03-17
* `DM`
* Can be used only in IL2CPP - DEBUG compilation mode.
* Added warning log about DM does not work without IL2CPP - DEBUG compilation mode.

## [0.5.6] - 2022-03-11
* `DependencyController`
* Added `DependencyIl2CppCompilerConfiguration`  class that allows you to define the compiler configuration via define.
* `DM`
* The sorting order for the canvas has been changed to max value.

## [0.5.5] - 2022-03-10
* `Save-Load System`
* Added ISaveLoadCallbackReceiver interface for receiving save and load messages.

## [0.5.4] - 2022-03-10
* `Object Pooler`
* Changed the path to the configuration files to the correct one..

## [0.5.3] - 2022-03-09
* `Save-Load System`
* Fixed save path for IOS.

## [0.5.2] - 2022-03-03
* `Extension`
* Added extensions for ObservableCollection: ContainsIndex(int), IsNullOrEmpty(), IsLastIndex(int)
* Replace namespaces in extensions classes from General to General.Extension

## [0.5.1] - 2022-03-03
* `Object Pooler`
* Fix a configuration files naming.

## [0.5.0] - 2022-02-22
* `Object Pooler`
* Add a Object Pooler plugin. Need a refactoring.

## [0.4.1] - 2022-02-17
* `DependencyController`
* Fixed API.

## [0.4.0] - 2022-02-17
* `DependencyController`
* Added full support Dependency Controller.
* Added to save state Dependency Controller.
* 
## [0.3.2] - 2022-02-21
* `Serialization`
* Added Serialized Observable Collection.

## [0.3.1] - 2022-02-14
* `Serialization`
* Added Observable and Serialized dictionary.

## [0.3.0] - 2022-02-11
* `DM`
* Added full support Debug Menu.

## [0.2.14] - 2022-02-08
* `Attribute`
* Fixed attribute `InterfaceCheckerEditor`, method TryFindInterface() - ArgumentOutOfRangeException.

## [0.2.13] - 2022-01-27
* `Mediator`
* Added a warning about the absence of object in the mediator.

## [0.2.12] - 2021-12-29
* `Extension`
* Array and List: IsLastIndex(int).
* Add static class 'Log' with methods NoEventListener() and NoEventListener(string eventName).

## [0.2.11] - 2021-12-26
* `Save-Load System`
* Fixed API.

## [0.2.10] - 2021-12-26
* `Save-Load System`
* Added autotests for the SaveLoad system.
* Refactoring `Save-Load System`.
* The public API was changed(SaveLoadComponent -> SLComponent).

## [0.2.9] - 2021-12-26
* `Mediator`
* `Extension`
* Object: IsNull(message).
* UnityObject: IsNull(message).
* Added checking of values for null in access.
* Refactoring `Mediator`.

## [0.2.8] - 2021-12-10
* `Extension`
* Boolean: Not()
* Rigidbody: ClampVelocity(float min, max), ClampVelocity(Vector3 min, max)
* Rigidbody2D: ClampVelocity(float min, max), ClampVelocity(Vector2 min, max)
* Vector2 and Vector3: Rotate(Quaternion), Rotate(Vector3), Add(float), Sub(float)
* UnityObject: LogIfNull(), IsNull(), NotNull(), DestroyNotNull()
* GameObject: Parent(Transform), Unparent(), Copy()
* Object: IsNull(), NotNull()
* Dictionary: AddOrReplace()
* Array and List: LastIndex(), SetFirst(T item), SetLast(T item), ContainsIndex(int index), Random(),
*   Empty(), NotEmpty(), Convert(), DoWithEveryone(), ParentAll(Transform parent), UnparentAll(), 
*   SetActiveAll(bool state), SetEnabled(bool state), DestroyAllNotNull()
* List only: RemoveLast()
* Image: Fade(float alpha)
* SpriteRenderer: Fade(float alpha)

## [0.2.7] - 2021-11-17
* `Mediator`
* Added the ability to overwrite a SingleComponent both forcibly and automatically.

## [0.2.6] - 2021-11-15
* `Mediator`|`Attribute`
* Refactoring module `Mediator`,`Attribute`.

## [0.2.5] - 2021-11-12
* `Save-Load System`
* Little refactoring module `Save-Load System`.

## [0.2.4] - 2021-11-11
* `Attribute`
* Fixed attributes `InterfaceCheckerEditor`.

## [0.2.3] - 2021-11-10
* `Attribute`
* Fixed attributes `ShowIf`, `HideIf`.

## [0.2.2] - 2021-11-04
* `Attribute`
* Added attributes `ReadOnly`,` ReadOnlyOnPlay`.

## [0.2.1] - 2021-11-03
* `Attribute`
* Added attributes `InterfaceChecker`,`ShowIf`,`HideIf`.

## [0.2.0] - 2021-11-01
* `Mediator`.
* Added the ability to fully use the `Components` in the `Mediator`.
* Added the ability to get items asynchronously.
* Added autotests for `Mediator`.
* Little refactoring.
* Added code documentation.

## [0.1.9] - 2021-10-08
* `Save-Load System`.
* Fixed a bug.

## [0.1.8] - 2021-10-08
## [0.1.7] - 2021-10-08
* `Save-Load System`.
* Fixed a bug related to the path now every time a new path is generated.

## [0.1.6] - 2021-10-07
* `Mediator`.
* Added the ability to fully use the `SingleComponent` in the `Mediator`.
* Fixed the problem related to the definition of the operator `==`.
* Implemented sorter by interface.
* Little refactoring.
* Added code documentation.

## [0.1.5] - 2021-10-06
* `Mediator`.
* Added the following classes interfaces for `Mediator`.
* `SingleComponent`,`MediatorSystem`,`MediatorComponent`,`ISingleComponent`,`IComponent`,`IMediator`,`SetMode`.

## [0.1.4] - 2021-10-06
* `Save-Load System`.
* Fixed a bug related to saving the path in the `JsonStrategy`.

## [0.1.3] - 2021-10-04
* `PlayerPrefsEditor`.
* The plugin was distributed among assemblies based on the need for execution.
* `Save-Load System`.
* `Data` struct access modifier has been changed.

## [0.1.2] - 2021-10-03
* `Save-Load System`.
* Added buffer for `DataAttribute`.

## [0.1.1] - 2021-09-30
* `PlayerPrefsEditor`.
* Integrated `PlayerPrefsEditor` into General plugin.

## [0.1.0] - 2021-09-30
* `Save-Load System`.
* Fixed a bug related to saving the path in the `JsonStrategy`.
* Refactoring module `Save-Load System`.

## [0.0.10] - 2021-09-29
* `Save-Load System`.
* Refactoring module `Save-Load System`.

## [0.0.9] - 2021-09-28
* `Save-Load System`.
* Added the ability to save and load in `PlayerPrefs` mode.
* Added the ability to encrypt in `PlayerPrefs` mode.
* Added additional ability to configure `DataAttribute`.
* Refactoring module `Save-Load System`.

## [0.0.8] - 2021-09-24
* `Save-Load System`.
* Refactoring module `Save-Load System`.

## [0.0.7] - 2021-09-23
* `Save-Load System`.
* Added the ability to encrypt in `XML` format.
* Added new methods <Decode,Encod> to the interface `IEncryption`.

## [0.0.6] - 2021-09-23
* `Save-Load System`.
* Added the ability to save and load in `XML` format.

## [0.0.5] - 2021-09-19
* `Save-Load System`.
* Added encryption option in `DataAttribute`.
* Also added support for encryption from the josn side.

## [0.0.1] - 2021-09-14
