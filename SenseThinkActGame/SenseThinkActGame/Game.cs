using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActGame
{
 
    public class Game
    {
        //TODO tierding
        public static double TierdingFactor = 1.0;
        public static double SensingDistance = 100.0;
        
        public enum DIRECTION { UP, DOWN, LEFT, RIGHT };
        private Dictionary<DIRECTION, Action> _directionToAction = new Dictionary<DIRECTION,Action>();
        private object _moveLockObj = new object();
        private Func<bool> _isGoalStatePredicate;
        
        public Game(Player bot, Player manual, List<Player> uninvolvedList, List<Entity> refugeList, Func<bool> isGoalStatePredicate, GameClock gameClock)
        {
            Bot = bot;
            Manual = manual;
            UninvolvedList = uninvolvedList;
            RefugeList = refugeList;
            _isGoalStatePredicate = isGoalStatePredicate;
            GameClock = gameClock;
            RegisterActionsToDirection();
        }

        private void RegisterActionsToDirection()
        {
            _directionToAction.Add(DIRECTION.DOWN,  (Action)(() => Manual.EntityState.Heading = 0.0));
            _directionToAction.Add(DIRECTION.LEFT,  (Action)(() => Manual.EntityState.Heading = 3.0 *  Math.PI / 2.0));
            _directionToAction.Add(DIRECTION.RIGHT, (Action)(() => Manual.EntityState.Heading = Math.PI / 2.0));
            _directionToAction.Add(DIRECTION.UP,    (Action)(() => Manual.EntityState.Heading = Math.PI));
        }
        
        public Player Bot { get; set; }
        public Player Manual { get; set; }
        public List<Player> UninvolvedList { get; set; }
        public List<Entity> RefugeList { get; set; }
        public GameClock GameClock { get; set; }
        public event EventHandler GameOver;

        
        public void Start()
        {
            InitGameClock();
        }

        private void InitGameClock(long interval = 0)
        {
            GameClock.Elapsed += GameClock_Elapsed;
        }
        public void Stop()
        {
            GameClock.Elapsed -= GameClock_Elapsed;
            GameClock.Stop();
        }

        public void MoveManual(DIRECTION dir)
        {
            Debug.Assert(Manual != null, "Manual player is null");
            _directionToAction[dir].Invoke();
            Manual.Play();
        }

        void GameClock_Elapsed(object sender, EventArgs e)
        {
            lock (_moveLockObj)
            {
                Task.Factory.StartNew(() => Bot.Play());
                
                Task.Factory.StartNew(() => Parallel.ForEach<Player>(UninvolvedList, (player) => player.Play()));

                if (GameOver != null && _isGoalStatePredicate != null && _isGoalStatePredicate.Invoke())
                    GameOver(this, EventArgs.Empty); 
            }
        }
    }   
}
