using UnityEngine;
public class Plate : Destroyable {

    public int shouldDestroy;
    private bool isColliding = false;

    public override void Start() {
        base.Start();
        shouldDestroy = 0;
        baseTraction = -10;
    }

    protected override void Destroy() {
        base.Destroy();
        
        transform.position -= new Vector3(0.0f, 1.0f, 0.0f);
    }

    public override void Update() {
        base.Update();

        if (shouldDestroy > 2 && !destroyed) {
            Destroy();


        } else if (!isColliding) {
            shouldDestroy++;
        } else {
            shouldDestroy = 0;
        }

        isColliding = false;

    }
    
    public override void OnTriggerStay(Collider other) {
        base.OnTriggerStay(other);
        isColliding = true;
    }

}