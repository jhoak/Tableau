using System;
using UnityEngine;
using UnityEngine.Events;

namespace Tableau.Base.Event {

    public class GeneralEvents {
        
        public static const UnityEvent LoadStart = new UnityEvent(),
                                       LoadEnd = new UnityEvent(),
                                       GameStart = new UnityEvent(),
                                       GameEnd = new UnityEvent(),
                                       GameRestart = new UnityEvent();

    }

}
