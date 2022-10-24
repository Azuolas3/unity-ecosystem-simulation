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
            GameObject childObject = Object.Instantiate(gameObject, gameObject.transform.position, Quaternion.identity);
            Animal child = childObject.GetComponent<Animal>();


            child.isGrownUp = false;
            child.Init(childObject, 20, 20, 20, 5, GenderHandler.GetRandomGender(child));
            Debug.Log("poof");
        }

        IEnumerator PregnancyCouroutine(int gestationPeriod)
        {
            yield return new WaitForSeconds(gestationPeriod);
            GiveBirth();
        }
    }
}
