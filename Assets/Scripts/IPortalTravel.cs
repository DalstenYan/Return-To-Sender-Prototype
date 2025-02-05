using UnityEngine;

public interface IPortalTravel
{
    bool IsTraveling { get; set; }

    public void FlipTraveling() {
        IsTraveling = !IsTraveling;
    }
}
