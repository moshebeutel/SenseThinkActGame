using SenseThinkActGame;
using SenseThinkActGame.Actions;
using SenseThinkActGame.ActuatorModels;
using SenseThinkActGame.Actuators;
using SenseThinkActGame.HighLevelDecisionMakers;
using SenseThinkActGame.Planners;
using SenseThinkActGame.Planners.MotionPlanners;
using SenseThinkActGame.WorldActionAppliers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SenseThinkActApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        internal static class Config
        {
            internal static void ReadConfig()
            {
                ReadConfig("TierdingFactor", out Game.TierdingFactor, 1.0);
                ReadConfig("SensingDistance", out Game.SensingDistance, 1000.0);
                ReadConfig("LargeAngularError", out ShootingActuatorModel.LargeAngularError, 0.3);
                ReadConfig("LargeShootingRangeError", out ShootingActuatorModel.LargeRangeError, 3.0);
                ReadConfig("StadartShootingRangeError", out ShootingActuatorModel.StandartRangeError, 1.0);
                ReadConfig("StadartShootingAngularError", out ShootingActuatorModel.StandartAngularError, 0.1);
                ReadConfig("DistanceFromGoalForGameOver", out DistanceFromGoalForGameOver, DistanceFromGoalForGameOver);
                ReadConfig("MinimumDistanceToEntity", out MinimumDistanceToEntity, MinimumDistanceToEntity);
                ReadConfig("DefaultClockInterval", out GameClockInterval, GameClockInterval);
                ReadConfig("TurningResolutionFactorOfPI", out TurningResolutionFactorOfPI, TurningResolutionFactorOfPI);
                ReadConfig("NumberOfRefuges", out NumberOfRefuges, NumberOfRefuges);
                ReadConfig("NumberOfUninvolved", out NumberOfUninvolved, NumberOfUninvolved);
                ReadConfig("NumberOfForwardLookingOfflineMoves", out NumberOfForwardLookingOfflineMoves, NumberOfForwardLookingOfflineMoves);
                ReadPositionFromConfig("TargetPositionX", "TargetPositionY", 700.0, 700.0, TargetPosition);
                ReadPositionFromConfig("BotPositionX", "BotPositionY", 50.0, 30.0, BotStartPosition);
                ReadPositionFromConfig("ManualPositionX", "ManualPositionY", 500.0, 300.0, ManualStartPosition);
            }

            internal static double   DistanceFromGoalForGameOver = 1.0;
            internal static double   MinimumDistanceToEntity = 0.1;
            internal static long     GameClockInterval = 100;
            internal static double   TurningResolutionFactorOfPI = 0.9;
            internal static int      NumberOfRefuges = 20;
            internal static int      NumberOfUninvolved = 20;
            internal static uint      NumberOfForwardLookingOfflineMoves = 10u;
            internal static Position TargetPosition = new Position();
            internal static Position BotStartPosition = new Position();
            internal static Position ManualStartPosition = new Position();


            private static void ReadConfig(string configStr, out double configVal, double defaultVal)
            {
                var configValueStr = ConfigurationManager.AppSettings[configStr];
                if (!double.TryParse(configValueStr, out configVal))
                    configVal = defaultVal;
            }
            private static void ReadConfig(string configStr, out int configVal, int defaultVal)
            {
                double val = 0.0;
                ReadConfig(configStr, out val, (double)defaultVal);
                configVal = (int)val;
            }

            private static void ReadConfig(string configStr, out long configVal, long defaultVal)
            {
                double val = 0.0;
                ReadConfig(configStr, out val, (double)defaultVal);
                configVal = (long)val;
            }

            private static void ReadConfig(string configStr, out uint configVal, uint defaultVal)
            {
                double val = 0.0;
                ReadConfig(configStr, out val, (double)defaultVal);
                configVal = (uint)val;
            }

            private static void ReadPositionFromConfig(string configXStr, string configYStr, double defaultX, double defaultY, Position pos)
            {
                double x = 0.0;
                ReadConfig(configXStr, out x, defaultX);
                double y = 0.0;
                ReadConfig(configYStr, out y, defaultY);
                pos.X = x;
                pos.Y = y;
            }
        }
        Game _game ;      
        static IDictionary<Shooting.AMMO_TYPE, IEnumerable<Tuple<double, double, double>>> ammoRangeRadiusDamagePercentage = new Dictionary<Shooting.AMMO_TYPE, IEnumerable<Tuple<double, double, double>>>
        {
            {
                Shooting.AMMO_TYPE.MEDIUM, new List<Tuple<double,double,double>>
                {  
                    new Tuple<double,double,double>(1.0, 0.1, 0.8),
                    new Tuple<double,double,double>(2.0, 0.1, 0.6),
                    new Tuple<double,double,double>(1.0, 0.2, 0.6),
                    new Tuple<double,double,double>(2.0, 0.2, 0.4),
                    new Tuple<double,double,double>(1.0, 0.3, 0.4),
                    new Tuple<double,double,double>(2.0, 0.3, 0.2),
                    new Tuple<double,double,double>(1.0, 0.4, 0.2),
                    new Tuple<double,double,double>(2.0, 0.4, 0.1)
                }
            }
        };
        static IDictionary<Shooting.AMMO_TYPE, double> maxRelevanceRadius = new Dictionary<Shooting.AMMO_TYPE, double>
        {
            {Shooting.AMMO_TYPE.LIGHT, 1.0},
            {Shooting.AMMO_TYPE.MEDIUM, 2.0},
            {Shooting.AMMO_TYPE.HEAVY, 3.0}
        };
     
        IWorld _world;
        Random _random = new Random();
        Dispatcher _dispatcher;
        EntityViewModel _targetViewModelEntity;
        private uint _idCounter = 0;

        public ICommand FwdCommand { get; set; }
        public ICommand BckdCommand { get; set; }
        public ICommand LeftCommand { get; set; }
        public ICommand RightCommand { get; set; }
        public ICommand ShootCommand { get; set; }

        public MainWindow()
        {
            InitCommands();

            InitGameAndEntities();
            
            InitTargetViewModelEntity();

            _dispatcher = Dispatcher.CurrentDispatcher;
           
            DataContext = this;
            
            InitializeComponent();

            InitDataTemplates();

            LaunchGame();
        }

        private void InitCommands()
        {
            FwdCommand = new MouseKeyCommand(MoveMeFwd);
            BckdCommand = new MouseKeyCommand(MoveMeBwd);
            LeftCommand = new MouseKeyCommand(MoveMeLeft);
            RightCommand = new MouseKeyCommand(MoveMeRight);
            ShootCommand = new MouseKeyCommand(Shoot);
        }

        private void Shoot()
        {
            _game.Manual.ReadyToShoot = true;
        }
        private void MoveMeFwd()
        {
            _game.MoveManual(Game.DIRECTION.UP);
        }
        private void MoveMeBwd()
        {
            _game.MoveManual(Game.DIRECTION.DOWN);
        }
        private void MoveMeLeft()
        {
            _game.MoveManual(Game.DIRECTION.LEFT);
        }
        private void MoveMeRight()
        {
            _game.MoveManual(Game.DIRECTION.RIGHT);
        }
        private void LaunchGame()
        {
            var timer = new Timer { Interval = Config.GameClockInterval };
            timer.Elapsed += (o, e) => UpdateEntities();
            timer.Start();
            _game.Start();
        }
        private void InitDataTemplates()
        {
            FindAndRegisterTemplate("uninvolvedTemplate", EntityViewModel.ENTITY_TYPE.UNINVOLVED);
            FindAndRegisterTemplate("botTemplate", EntityViewModel.ENTITY_TYPE.BOT);
            FindAndRegisterTemplate("refugeTemplate", EntityViewModel.ENTITY_TYPE.REFUGE);
            FindAndRegisterTemplate("targetTemplate", EntityViewModel.ENTITY_TYPE.TARGET);
            FindAndRegisterTemplate("manualTemplate", EntityViewModel.ENTITY_TYPE.MANUAL);
        }
        private void InitTargetViewModelEntity()
        {
            _targetViewModelEntity = new EntityViewModel { EntityType = EntityViewModel.ENTITY_TYPE.TARGET, X = Config.TargetPosition.X, Y = Config.TargetPosition.Y };
        }
        private void InitGameAndEntities()
        {
            Config.ReadConfig();
            
            var world = new World(new List<IWorldActionApplier> { new MotionWorldActionApplier(Config.MinimumDistanceToEntity), new ShootingWorldActionApplier(ammoRangeRadiusDamagePercentage, maxRelevanceRadius) });

            _world = world;

            var bot = InitBotPlayer();

            var manual = InitManualPlayer();

            var uninvolvedList = InitUninvolvedList();

            var refugeList = InitRefugeList();
            //TODO game should get world or worldstate
            _game = new Game(bot, manual, uninvolvedList, refugeList, () => bot.EntityState.Position.DistanceFrom(Config.TargetPosition) < Config.DistanceFromGoalForGameOver, new GameClock(Config.GameClockInterval));
            _game.GameOver += OnGameOver;

            world.RealWorldState = new WorldState(bot.EntityState, manual.EntityState, uninvolvedList.Select(ent => ent.EntityState), refugeList.Select(ent => ent.EntityState));
        }

        private Player InitBotPlayer()
        {
            var sensor = new Sensor(new SensorModel(Game.SensingDistance, 0.0, 0.0, 0.0));
            var planners = new List<IActionPlanner> { new TargetedMotionPlanner(Config.TargetPosition, Config.TurningResolutionFactorOfPI * Math.PI) };

            var offlineGameClock = new GameClock();
            var offlineGame = new Game(null, null, new List<Player>(), new List<Entity>(), null, offlineGameClock);

            var adm = new HighLevelDecisionMaker(null, offlineGame, offlineGameClock, planners, new PlayerFactory(_world), Config.NumberOfForwardLookingOfflineMoves);
            var motionActuator = new MotionActuator(MotionActuatorModel.Perfect);        
            var shootingActuator = new ShootingActuator(ShootingActuatorModel.Standart);

            //TODO enforce gameentity and id in ctor
            return new Player(_world, Config.BotStartPosition, sensor, new List<IActuator> { motionActuator }, adm) {  GameIdentity = Entity.GAME_IDENTITY.BOT, Id = ++_idCounter };
        }

        private Player InitManualPlayer()
        {
            //return null;
            var sensor = new Sensor(new SensorModel(Game.SensingDistance, 0.0, 0.0, 0.0));
            //TODO manual does not need decision maker. ManualPlayer should inherit from player 
            var adm = new SimpleHighLevelDecisionMaker(null, new List<IActionPlanner> { new SimpleMotionPlanner() });
            var motionActuator = new MotionActuator(MotionActuatorModel.Perfect);
            var shootingActuator = new ShootingActuator(ShootingActuatorModel.Standart);

            return new Player(_world, Config.ManualStartPosition, sensor, new List<IActuator> { motionActuator, shootingActuator }, adm) {GameIdentity = Entity.GAME_IDENTITY.MANUAL, Id = ++_idCounter };
        }

        private List<Entity> InitRefugeList()
        {
            var refugeList = new List<Entity>();
            for (int i = 0; i < Config.NumberOfRefuges; i += 2)
            {
                refugeList.Add(new Refuge { EntityState = new EntityState {Position = new Position { X = _random.Next(800), Y = _random.Next(800) }}, RefugeType = Refuge.REFUGE_TYPE.FIRM , Id = ++_idCounter});
                refugeList.Add(new Refuge { EntityState = new EntityState { Position = new Position { X = _random.Next(700), Y = _random.Next(700) } }, RefugeType = Refuge.REFUGE_TYPE.SOFT, Id = ++_idCounter });
            }
            return refugeList;
        }

        private List<Player> InitUninvolvedList()
        {
            var uninvolvedList = new List<Player> ();
            for (var i = 0U; i < Config.NumberOfUninvolved; i++)
			{
                uninvolvedList.Add(InitUninvolved());
			}
            return uninvolvedList;
        }

        private Player InitUninvolved()
        {
            var sensor = new Sensor(SensorModel.Perfect);
            var adm = new SimpleHighLevelDecisionMaker( null, new List<IActionPlanner>{new RandomMotionPlanner()});
            var motionActuator = new MotionActuator (MotionActuatorModel.Perfect );

            return new Player(_world, new Position { X = _random.Next(800), Y = _random.Next(1200) } ,sensor, new List<IActuator> { motionActuator }, adm) {  GameIdentity = Entity.GAME_IDENTITY.UNINVOLVED, Id = ++_idCounter };
        }

        void OnGameOver(object sender, EventArgs e)
        {
            _game.Stop();
            var res = MessageBox.Show("GAME OVER!");
            _dispatcher.Invoke((Action)(()=>Application.Current.Shutdown()));
        }

        private void FindAndRegisterTemplate(string templatename, EntityViewModel.ENTITY_TYPE entityType)
        {
            var template = FindResource(templatename) as DataTemplate;
            Debug.Assert(template != null);
            EntityTemplateSelector.RegisterDataTemplate(entityType, template);
        }

        private void UpdateEntities()
        {
            Task.Factory.StartNew(() =>
            {
                var entitiesList = new List<EntityViewModel>();
                entitiesList.Add( GetEntity(_game.Bot, EntityViewModel.ENTITY_TYPE.BOT));
                entitiesList.Add(GetEntity(_game.Manual, EntityViewModel.ENTITY_TYPE.MANUAL));
                _game.UninvolvedList.ForEach(player => entitiesList.Add(GetEntity(player, EntityViewModel.ENTITY_TYPE.UNINVOLVED)));
                _game.RefugeList.ForEach(refuge => entitiesList.Add(GetEntity(refuge, EntityViewModel.ENTITY_TYPE.REFUGE)));
                entitiesList.Add(_targetViewModelEntity);
                Entities = entitiesList;
            });
        }

        private static EntityViewModel GetEntity(Entity entity, EntityViewModel.ENTITY_TYPE entityType)
        {
            return new EntityViewModel { X = entity.EntityState.Position.X, Y = entity.EntityState.Position.Y ,Heading = entity.EntityState.Heading, EntityType = entityType, State = entity.EntityState.Strength};
        }

        private List<EntityViewModel> _entities;

        public List<EntityViewModel> Entities
        {
            get { return _entities; }
            set { _entities = value; RaisePropertyChanged("Entities"); }
        }


        private void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));

        }
        public event PropertyChangedEventHandler PropertyChanged;
        
    }

    public class EntityTemplateSelector: DataTemplateSelector
    {
        public static void RegisterDataTemplate(EntityViewModel.ENTITY_TYPE entityType, DataTemplate template)
        {
            Debug.Assert(!_typeToTemplate.ContainsKey(entityType), "Type " + entityType + " is already registered");
            _typeToTemplate.Add(entityType, template);
        }
        static Dictionary<EntityViewModel.ENTITY_TYPE, DataTemplate> _typeToTemplate = new Dictionary<EntityViewModel.ENTITY_TYPE, DataTemplate>(); 
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Debug.Assert(item != null && item is EntityViewModel);
            var evm = item as EntityViewModel;
            Debug.Assert(_typeToTemplate.ContainsKey(evm.EntityType), "Unknown Entity Type");
            return _typeToTemplate[evm.EntityType];
        }
    }

    public class MouseKeyCommand : ICommand
    {
        Action _action;

        public MouseKeyCommand(Action action)
	    {
            _action = action;
	    }


        public bool CanExecute(object parameter)
        { 
            Debug.WriteLineIf(CanExecuteChanged == null, "");
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
           
            _action.Invoke();
        }
    }

}
