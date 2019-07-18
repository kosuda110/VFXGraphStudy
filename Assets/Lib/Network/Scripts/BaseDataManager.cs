using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kosu.UnityLibrary
{
    public abstract class BaseDataManager<T> : Singleton<T> where T : Singleton<T>
    {

        protected ISender[] _senders;

        protected IReciever[] _recievers;

        public override bool IsDontDestroyOnLoad => true;

        private void OnEnable()
        {
            Setup();
        }

        private void OnDisable()
        {
            BeforeClose();

            foreach (var sender in _senders)
            {
                sender.Close();
            }

            foreach (var reciever in _recievers)
            {
                reciever.Close();
            }

            _senders = null;
            _recievers = null;
        }

        protected abstract void Setup();

        protected abstract void BeforeClose();

    }
}
