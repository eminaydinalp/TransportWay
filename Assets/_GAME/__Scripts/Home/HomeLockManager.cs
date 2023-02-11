using System.Collections.Generic;
using System.Linq;
using Rentire.Core;
using Rentire.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace _GAME.__Scripts.Home
{
    public class HomeLockManager : Singleton<HomeLockManager>
    {
        public List<HomeLockController> homeLockControllers = new List<HomeLockController>();
        public static event UnityAction OnHomeUnlocked;

        private void Start()
        {
            for (int i = 0; i < homeLockControllers.Count; i++)
            {
                if (LocalPrefs.GetBool(homeLockControllers[i].levelHomePref2))
                {
                    homeLockControllers[i].gameObject.SetActive(true);
                }
            }
        }

        public void InvokeOnHomeUnlocked()
        {
            UserPrefs.SetMaxHomeLevel(UserPrefs.GetMaxHomeLevel() + 1);
            UserPrefs.Save();
            OnHomeUnlocked?.Invoke();
        }

        public void OpenNewHomeLock()
        {
            var newHomeLockController = homeLockControllers.FirstOrDefault(x => !LocalPrefs.GetBool(x.levelHomePref2));

            if (newHomeLockController == null) return;
            
            newHomeLockController.gameObject.SetActive(true);
            newHomeLockController!.SetCameraSize();
            newHomeLockController.OpenHomeLock();
            InvokeOnHomeUnlocked();
            CallMethodWithDelay(TutorialManager.Instance.ShowNewAreaWithHand, 0.5f);
        }
    }
}