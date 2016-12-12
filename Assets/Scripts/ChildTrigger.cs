using UnityEngine;

public class ChildTrigger : MonoBehaviour {
        public void OnTriggerEnter(Collider other) {
                if (other.gameObject.transform.parent.gameObject != transform.parent.gameObject) {
                        // Debug.Log(other.gameObject.transform.parent.gameObject);
                        transform.parent.gameObject.GetComponent<Thing>().OnTriggerEnter(other);
                }
        }

        public void OnTriggerStay(Collider other) {
            //Debug.Log(other.gameObject.transform.parent.gameObject.name);
                if (other.gameObject.transform.parent.gameObject != transform.parent.gameObject) {
                        // Debug.Log(other.gameObject.transform.parent.gameObject);
                        transform.parent.gameObject.GetComponent<Thing>().OnTriggerStay(other);
                }
        }

        public void OnTriggerExit(Collider other) {
                if (other.gameObject.transform.parent.gameObject != transform.parent.gameObject) {
                        // Debug.Log(other.gameObject.transform.parent.gameObject);
                        transform.parent.gameObject.GetComponent<Thing>().OnTriggerExit(other);
                }
        }
}
