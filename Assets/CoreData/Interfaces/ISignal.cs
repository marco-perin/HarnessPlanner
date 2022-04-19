using UnityEngine;

namespace Assets.CoreData.Interfaces
{
    public interface ISignal : INamed, IWithId
    {
        public Color Color { get; set; }
    }
}