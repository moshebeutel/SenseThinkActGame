using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenseThinkActApp
{
    public class EntityViewModel : INotifyPropertyChanged
    {
        private double _heading;

        public double Heading
        {
            get { return _heading; }
            set { _heading = value; RaisePropertyChanged("Heading"); }
        }


        private double _x;

        public double X
        {
            get { return _x; }
            set { _x = value; RaisePropertyChanged("X"); }
        }

        private double _y;

        public double Y
        {
            get { return _y; }
            set { _y = value; RaisePropertyChanged("Y"); }
        }

        private double _state;

        public double State
        {
            get { return _state; }
            set { _state = value; RaisePropertyChanged("State"); }
        }


        public enum ENTITY_TYPE { BOT, MANUAL, UNINVOLVED, REFUGE, TARGET };
        public ENTITY_TYPE EntityType { get; set; }

        private void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));

        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
