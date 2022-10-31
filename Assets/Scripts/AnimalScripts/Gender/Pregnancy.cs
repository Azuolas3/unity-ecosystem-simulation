using System.Collections;
using UnityEngine;


namespace EcosystemSimulation
{
    public class Pregnancy : MonoBehaviour
    {
        public void CallPregnancyCouroutine(int gestationPeriod)
        {
            StartCoroutine(PregnancyCouroutine(gestationPeriod));
        }

        void GiveBirth()
        {
            GameObject childObject = Instantiate(gameObject, gameObject.transform.position, Quaternion.identity);
            Animal child = childObject.GetComponent<Animal>();


            child.isGrownUp = false;
            child.Init(childObject, 20, 20, 3, 10, 0.25f, GenderHandler.GetRandomGender(child), Color.grey);
            childObject.transform.SetParent(gameObject.transform.parent);
            Debug.Log("poof");
        }

        IEnumerator PregnancyCouroutine(int gestationPeriod)
        {
            yield return new WaitForSeconds(gestationPeriod);
            GiveBirth();
        }
    }
}
