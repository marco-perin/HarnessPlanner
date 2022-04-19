using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.UXData.Interfaces
{
    public interface IInteractable { }

    public interface IClickable : IInteractable
    {
        void Click();
    }

    public interface IClickable<Tparam> : IInteractable
    {
        void Click(Tparam param);
    }
    public interface IClickableV3 : IClickable<Vector3> { }

    public interface IDraggable : IInteractionStartableV3
    {
        void Drag(Vector3 dx);
    }

    public interface IInteractionStartable : IInteractable
    {
        void StartInteraction();
    }

    public interface IInteractionStartableV3 : IInteractionStartable<Vector3> { }

    public interface IInteractionStartable<Tparam> : IInteractable
    {
        void StartInteraction(Tparam param);
    }

    public interface IInteractionEndable : IInteractable
    {
        void EndInteraction();
    }

    public interface IHoverable : IInteractable
    {
        void Focus();
        void Unfocus();
    }
}
