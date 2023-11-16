# Game Kit

## [0.4.3] - 2023-01.27
* `Extensions`
* Fix IntExtensions and ProgressBar.
* Added StringBuilder for IntExtensions.

## [0.4.2] - 2023-01.27
* `General`
* Added GKParticlesFabric for simple particles spawning.
* Added SimpleParticle for simple particle control.
* Added IAdProvider and IAdTransaction interface.
* Added SimpleRequestRewardAdCommand as simple way to get Rewarded Ad.
* Minor fix Timer and OverlayPointer classes.

## [0.4.1] - 2022-12.15
* `UI & Extensions & Transaction System`
* Added CommonScreenFabric and ScreenBase 
* Added SafeArea script.
* Modified UnityObjectExtension and added IntExtensions.
* Remove Dependency class in TransactionSystem.
* Modified TransactionSystem tests.

## [0.3.2] - 2022-10.11
* `General`
* Added ExtendedObjectPooler with return of DontDestroyOnload-pools when calling SceneManager.activeSceneChanged.

## [0.3.1] - 2022-09.30
* `General`
* Update namespaces after update general plugin.
* Added observable components support in Transaction System Dependency.

## [0.2.4] - 2022-09.30
* `Tutorial Module`
* Added AttachedToTransformIndicator
* Refactor TutorialIndicator
* Fix OverlayPointer

## [0.2.3] - 2022-09.01
* `Transaction system & Levels System`
* A transaction strategy generator has been added to automate the writing of strategies.
* Added the Create NewLevel method to the Levels System to create a level by index.

## [0.2.2] - 2022-08.30
* `Tutorial Module`
* Added OverlayPointer

## [0.2.1] - 2022-08.19
* `Extensions & Levels System & Transaction System`
* Migrate unity-specific extensions methods from general-plugin.
* Fix logic of Levels System.
* Added method RemoveTransaction() to Transaction System.

## [0.1.4] - 2022-08.10
* `Levels System & Transaction System & Building Controller`
* Added LevelSystem with levels loading by strategies.
* Refactor TransactionSystem and examples.
* BuildingController refactoring

## [0.1.3] - 2022-07-22
* `TransactionSystem`
* In the TransactionSystem() constructor, you need to pass a sheet, not a dictionary, for ease of use.

## [0.1.2] - 2022-07-14
* `Tutorial Module`
* Added TutorialIndicator
* Removed default HesitatingPointer scale

## [0.1.1] - 2022-07-05
* `TransactionSystem`
* Added automated Transactions system with methods GetTransaction<T> and AddStrategy.

## [0.0.18] - 2022-06-06
* `Craft Module`
* Added BuildingController (with requirement interfaces). Designed to simplify the work with the building system.

## [0.0.17] - 2022-05-30
* `Tutorial Module`
* Added ITutorialController + ITutorialPart interfaces.
* Added simple variant of TutorialController.
* Added InteractWithResourceReceiverTutorialPart and InteractWithResourceSenderTutorialPart (examples for ITutorialPart).
* Added ITutorialPointer (HesitatingPointer, ParticlePointer)

## [0.0.16] - 2022-05-23
* `Craft Module`
* Uncommented correct code in ResourceType.cs

## [0.0.15] - 2022-05-20
* `Craft Module`
* ByColumnInteractingZone.cs - Component of the zone, to extract resources from which you can interact with the nearest column.
* Minor refactoring of ResourceType module.

## [0.0.14] - 2022-05-19
* `Craft Module`
* Added options for inheritance in ResourcesZone

## [0.0.13] - 2022-05-11
* `Craft Module`
* Refactor: IResourceReceiver, IResourceSender.
* Fix: minor bugs in ResourcesZone, BackpackPreferences
* ResourcesCreator, ResourcesDestroyer, ResourcesGenerator configured for inheritance.

## [0.0.12] - 2022-04-21
* `Craft Module`
* Added ResourcesCreator, ResourcesDestroyer, ResourcesGenerator mono-components.

## [0.0.11] - 2022-04-19
* `Craft Module`
* Added BackpackBase class with basic initialization and methods.

## [0.0.10] - 2022-04-11
* `Craft Module`
* Added module ResourcesTransferHelper with submodules SendResourcesHelper and AcceptResourcesHelper.
* SendResourcesHelper is responsible for the logic of sending resources.
* AcceptResourcesHelper is responsible for the logic of accepting resources.
* Both submodules provide full control over the process at all stages, and can also be used without ResourcesTransferHelper.
* ResourcesTransferHelper includes all the functionality of the two submodules and provides convenient access to them.

## [0.0.9] - 2022-04-07
* `Craft Module`
* Added constructor for ResourceType.

## [0.0.8] - 2022-04-04
* `Craft Module`
* Changed the async method in Resource Converter to a coroutine.

## [0.0.7] - 2022-03-30
* `Craft Module`
* Made ResourceType as IEquatable<ResourceType>.

## [0.0.6] - 2022-03-25
* `Craft Module`
* Added tested resources zones (Delivery zone + Storage zone).
* Added base Resource realisation and extensions.

## [0.0.5] - 2022-03-24
* `Craft Module`
* Added a ResourceType Component instead of "ResourceType" enum.
* ResourcesCreator<TComponent, TMatcher>: remove confines for TComponent and TMatcher.

## [0.0.4] - 2022-03-23
* `General`
* Added a ApplicationSettings component.

## [0.0.3] - 2022-03-17
* `Craft Module`
* Added a Upgrade System with the addition and improvement of objects. Added interface IUpgradable.

## [0.0.2] - 2022-03-15
* `Craft Module`
* Collision Resolver component : component for pushing objects when colliders penetrate each other.
* Resources Creator component : generic component for creating resources, it works based on <TComponent, TMatcher>, where TMatcher is an object type identifier, TComponent is UnityEngine.Component.
* Collision Listener component : component for tracking Сollisions.
* Trigger Listener component : component for tracking Triggers.

## [0.0.1] - 2022-02-21
