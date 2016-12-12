using UnityEngine;
using System.Collections.Generic;

public class Hauntable : Thing {

    private AudioClip hauntSound;

    public static Hauntable FindNearestToGhost() {
        Hauntable minHauntable = null;
        double minDist = float.MaxValue;

        LinkedList<Object> hauntableObjects = new LinkedList<Object>(FindObjectsOfType(typeof(Togglable)));
        
        Object[] destroyableObjects = FindObjectsOfType(typeof(Destroyable));
        foreach (Destroyable destroyable in destroyableObjects) {
            if (!destroyable.destroyed) {
                hauntableObjects.AddLast(destroyable);
            }
        } 

        foreach (Hauntable hauntable in hauntableObjects) {
            if ((hauntable.distanceFromGhost < Config.Instance.HAUNT_RADIUS) && (hauntable.distanceFromGhost < minDist)) {
                minDist = hauntable.distanceFromGhost;
                minHauntable = hauntable;
            }
        }

        return minHauntable;
    }
			
    public virtual void Haunt() {}

    protected override AudioClip SelectSound() {
		return hauntSound;
	}

}
