using Scripts.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Core.Managers
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;
        public static UIManager Instance => _instance;
        private Dictionary<Type, INetworkUI> _networkUIs;
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(this);

            _networkUIs = new Dictionary<Type, INetworkUI>();
            GetComponentsInChildren<INetworkUI>().ToList().ForEach(item => _networkUIs.Add(item.GetType(), item));
        }

        public T GetCompo<T>(bool isDerived = false) where T : INetworkUI
        {
            if (_networkUIs.TryGetValue(typeof(T), out INetworkUI component))
                return (T)component;
            if (isDerived == false) return default;

            Type findType = _networkUIs.Keys.FirstOrDefault(type => type.IsSubclassOf(typeof(T)));
            if (findType != null)
                return (T)_networkUIs[findType];

            return default;
        }

    }
}
