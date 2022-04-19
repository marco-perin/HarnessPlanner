using Assets.CoreData.Interfaces;
using UnityEngine;

namespace Assets.CoreData.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Signal", menuName = "CoreDataSO/Signal")]
    public class SignalSO : ScriptableObject, ISignal
    {
        [SerializeField]
        private string id;

        [SerializeField]
        private Color color;

        [SerializeField]
        private string _name;

        public Color Color { get => color; set => color = value; }
        public string Name { get => _name; set => _name = value; }
        public string Id { get => id; set => id = value; }
    }

}