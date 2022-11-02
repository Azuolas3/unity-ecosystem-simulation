using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EcosystemSimulation
{
    public class MaleHandler : GenderHandler
    {
        public MaleHandler(Animal animal)
        {
            this.baseAnimal = animal;
            Gender = AnimalGender.Male;
        }

        public override Action HandleReproductionPriority(Func<Collider[]> getColliders)
        {
            if (getColliders().Length != 0)
            {
                foreach (Collider collider in getColliders())
                {
                    Animal matingAnimal = collider.GetComponent<Animal>();
                    if (IsPartnerAppropiate(matingAnimal))
                    {
                        if(matingAnimal.currentAction != null)
                            matingAnimal.currentAction.Cancel();

                        matingAnimal.genderHandler.hasMate = true;
                        matingAnimal.currentDestination = matingAnimal.gameObject.Position();
                        matingAnimal.currentAction = null;
                        Debug.Log($"mating {matingAnimal.gameObject.name} and {baseAnimal.gameObject.name}");
                        //Quaternion rotationQuaternion = GetRotationQuaternion(baseAnimal.gameObject.Position(), matingAnimal.gameObject.Position());
                        Transform partnerTransform = matingAnimal.gameObject.transform;
                        //partnerTransform.rotation = Quaternion.Slerp(partnerTransform.rotation, rotationQuaternion, Time.deltaTime * 2);
                        //partnerTransform.LookAt(baseAnimal.gameObject.Position());
                        CouroutineHelper.Instance.CallCouroutine(RotateToDirection(partnerTransform, baseAnimal.gameObject.Position(), 0.5f));
                        
                        return new MatingAction(baseAnimal, matingAnimal);
                    }
                }
                return new MoveToAction(baseAnimal, baseAnimal.GetSearchDestination());
            }
            else
            {
                return new SearchAction(baseAnimal, getColliders, baseAnimal.GetSearchDestination());
            }
        }

        public override void HandleMating(Genes partnerGenes) { }

        private bool IsPartnerAppropiate(Animal partner)
        {
            return (partner.currentPriority == Animal.Priority.Reproduce && partner.genderHandler.Gender == AnimalGender.Female && !partner.genderHandler.hasMate);
        }

        //private Quaternion GetRotationQuaternion(Vector3 position, Vector3 fromPosition)
        //{
        //    Vector3 direction = (position - fromPosition).normalized;
        //    return Quaternion.LookRotation(direction);
        //}

        public IEnumerator RotateToDirection(Transform transform, Vector3 positionToLook, float timeToRotate)
        {
            Quaternion startRotation = transform.rotation;
            Vector3 direction = positionToLook - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            float time = 0f;

            while (time <= 1f)
            {
                time += Time.deltaTime / timeToRotate;
                transform.rotation = Quaternion.Lerp(startRotation, lookRotation, time);
                yield return null;
            }
            transform.rotation = lookRotation;
        }

        public override bool IsAvailableForMating()
        {
            return baseAnimal.IsGrownUp;
        }
    }
}
