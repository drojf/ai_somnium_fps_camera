//Merge the following class with the existing Game.RootNode class

class RootNode {
    public void ModPause()
    {
        if (!this.pause)
        {
            this.pauseState = new RootNode.PauseState();
            this.pause = true;
            this.pauseState.monoBehaviours.Clear();
            //Modified so that the character's eyes and eyelids are frozen, but all other monobehaviors still run
            foreach (MonoBehaviour monoBehaviour in base.GetComponentsInChildren<MonoBehaviour>(true))
            {
                if (monoBehaviour != null && (monoBehaviour.GetType() == typeof(EyeMove) || monoBehaviour.GetType() == typeof(EyeController)) && monoBehaviour.enabled)
                {
                    monoBehaviour.enabled = false;
                    this.pauseState.monoBehaviours.Add(monoBehaviour);
                }
            }
            DynamicBone[] componentsInChildren2 = base.GetComponentsInChildren<DynamicBone>(true);
            for (int i = 0; i < componentsInChildren2.Length; i++)
            {
                componentsInChildren2[i].pause = true;
            }
            this.pauseState.cameras.Clear();
            foreach (Animator animator in base.GetComponentsInChildren<Animator>(true))
            {
                if (animator != null && animator.enabled)
                {
                    animator.enabled = false;
                    this.pauseState.animators.Add(animator);
                }
            }
            this.pauseState.audioSources.Clear();
            foreach (AudioSource audioSource in base.GetComponentsInChildren<AudioSource>(true))
            {
                if (audioSource != null && audioSource.enabled && audioSource.isPlaying)
                {
                    audioSource.Pause();
                    this.pauseState.audioSources.Add(audioSource);
                }
            }
            this.pauseState.customAnimators.Clear();
            foreach (CustomAnimator customAnimator in base.GetComponentsInChildren<CustomAnimator>(true))
            {
                if (customAnimator != null)
                {
                    customAnimator.Pause(true);
                    this.pauseState.customAnimators.Add(customAnimator);
                }
            }
            this.pauseState.playableDirectors.Clear();
            foreach (PlayableDirector playableDirector in base.GetComponentsInChildren<PlayableDirector>(true))
            {
                if (playableDirector != null && playableDirector.gameObject.activeInHierarchy && playableDirector.enabled && playableDirector.state == PlayState.Playing)
                {
                    CutSceneHelper component = playableDirector.GetComponent<CutSceneHelper>();
                    if (component != null)
                    {
                        if (!component.pause)
                        {
                            component.ui_pause = true;
                            component.ui_time = playableDirector.time;
                            component.enabled = true;
                        }
                    }
                    else
                    {
                        playableDirector.Pause();
                    }
                    this.pauseState.playableDirectors.Add(playableDirector);
                }
            }
            this.pauseState.particleSystems.Clear();
            foreach (ParticleSystem particleSystem in base.GetComponentsInChildren<ParticleSystem>(true))
            {
                if (particleSystem != null && particleSystem.gameObject.activeSelf && particleSystem.isPlaying)
                {
                    particleSystem.Pause();
                    this.pauseState.particleSystems.Add(particleSystem);
                }
            }
            this.pauseState.rigidbodys.Clear();
            foreach (Rigidbody rigidbody in base.GetComponentsInChildren<Rigidbody>(true))
            {
                if (rigidbody != null)
                {
                    RootNode.RigidbodyState rigidbodyState = new RootNode.RigidbodyState();
                    rigidbodyState.rigidbody = rigidbody;
                    rigidbodyState.velocity = rigidbody.velocity;
                    rigidbodyState.angularVelocity = rigidbody.angularVelocity;
                    rigidbody.Sleep();
                    this.pauseState.rigidbodys.Add(rigidbodyState);
                }
            }
        }
    }
}