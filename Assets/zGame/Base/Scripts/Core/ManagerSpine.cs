// using Spine;
// using Spine.Unity;
// using Spine.Unity.AttachmentTools;
using System;
using UnityEngine;

// by nt.Dev93
namespace ntDev
{
    class AnimData
    {
        public string name;
        public bool loop;
        public bool playAgain;
        public int track;
        public float atTime;

        public AnimData(string name, bool loop, bool playAgain, int track, float atTime)
        {
            this.name = name;
            this.loop = loop;
            this.playAgain = playAgain;
            this.track = track;
            this.atTime = atTime;
        }
    }
    public class ManagerSpine : MonoBehaviour
    {
        // SkeletonAnimation skeAnim;
        // SkeletonGraphic skeGraph;

        // Spine.AnimationState anim;
        // Skeleton ske;
        // public Skeleton Ske => ske;

        [SerializeField] bool StandAlone;
        float currentTimeScale = 1;
        float factorTimeScale = 1;

        Action<string>[] eventCallback = new Action<string>[5];
        Action[] doneCallback = new Action[5];

        int status = 0;
        int Status
        {
            get
            {
                if (status == 0)
                {
                    // skeAnim = GetComponent<SkeletonAnimation>();
                    // if (skeAnim == null)
                    // {
                    //     skeGraph = GetComponent<SkeletonGraphic>();
                    //     if (skeGraph == null) status = -1;
                    //     else
                    //     {
                    //         anim = skeGraph.AnimationState;
                    //         ske = skeGraph.Skeleton;
                    //         status = 2;
                    //         InitEvent();
                    //     }
                    // }
                    // else
                    // {
                    //     anim = skeAnim.AnimationState;
                    //     ske = skeAnim.Skeleton;
                    //     status = 1;
                    //     InitEvent();
                    // }
                    return status;
                }
                else return status;
            }
        }

        void InitEvent()
        {
            // if (anim != null)
            // {
            //     anim.Event += RaiseAnimEvent;
            //     anim.Complete += RaiseDoneEvent;
            // }
            // else status = 0;
        }

        public float TimeScaleFactor
        {
            get => factorTimeScale;
            set
            {
                factorTimeScale = value;
                // if (Status == 1) skeAnim.timeScale = currentTimeScale * factorTimeScale;
                // else if (Status == 2) skeGraph.timeScale = currentTimeScale * factorTimeScale;
            }
        }

        public float TimeScale
        {
            get => currentTimeScale;
            set
            {
                currentTimeScale = StandAlone ? 1 : value;
                // if (Status == 1) skeAnim.timeScale = currentTimeScale * factorTimeScale;
                // else if (Status == 2) skeGraph.timeScale = currentTimeScale * factorTimeScale;
            }
        }

        public string AnimName(int track = 0)
        {
            try
            {
                // if (Status > 0 && anim.Tracks != null && anim.Tracks.Count > track)
                // {
                //     if (anim.Tracks.Items[track] != null && anim.Tracks.Items[track].Animation != null) return anim.Tracks.Items[track].Animation.Name;
                //     return "";
                // }
                // else 
                return "";
            }
            catch (Exception e)
            {
                Ez.Log(e.ToString());
                return "";
            }
        }

        public void Clear(int t = -1)
        {
            // if (Status < 1) return;
            // if (t > -1)
            // {
            //     if (t < 5)
            //     {
            //         eventCallback[t] = null;
            //         doneCallback[t] = null;
            //     }
            //     anim.SetEmptyAnimation(t, 0);
            // }
            // else
            // {
            //     for (int i = 0; i < 5; ++i)
            //     {
            //         eventCallback[i] = null;
            //         doneCallback[i] = null;
            //         anim.SetEmptyAnimation(i, 0);
            //     }
            // }
        }

        void Start()
        {
            ManagerGame.AddSpine(this);
        }

        void OnDestroy()
        {
            ManagerGame.RemoveSpine(this);
        }

        AnimData animData;
        public void PlayAnim(string animName, bool loop = true, bool playAgain = true, int track = 0, float atTime = -1)
        {
            // try
            // {
            //     animData = null;
            //     doneCallback[track] = null;
            //     eventCallback[track] = null;

            //     if (gameObject.activeInHierarchy && Status > 0)
            //     {
            //         if (playAgain || (!playAgain && AnimName(track) != animName))
            //         {
            //             if (CheckAnim(animName))
            //             {
            //                 if (anim == null) return;
            //                 anim.SetAnimation(track, animName, loop);
            //                 if (atTime > 0) SetTime(track, atTime);
            //                 TimeScale = currentTimeScale;
            //                 gameObject.layer = 0;
            //             }
            //         }
            //     }
            //     else animData = new AnimData(animName, loop, playAgain, track, atTime);
            //     gameObject.SetActive(true);
            // }
            // catch (Exception) { }
        }

        void OnEnable()
        {
            if (animData != null) PlayAnim(animData.name, animData.loop, animData.playAgain, animData.track, animData.atTime);
        }

        public bool CheckAnim(string animName)
        {
            // if (ske == null || ske.Data == null) 
            return false;
            // return ske.Data.FindAnimation(animName) != null;
        }

        public void SetDoneEvent(Action act, int track = 0)
        {
            if (track < doneCallback.Length) doneCallback[track] = act;
        }

        public void SetAnimEvent(Action<string> act, int track = 0)
        {
            if (track < eventCallback.Length) eventCallback[track] = act;
        }

        // void RaiseDoneEvent(TrackEntry trackE)
        // {
        //     if (trackE.Animation.Name != AnimName(trackE.TrackIndex)) return;
        //     doneCallback[trackE.TrackIndex]?.Invoke();
        // }

        // void RaiseAnimEvent(TrackEntry trackE, Spine.Event e)
        // {
        //     eventCallback[trackE.TrackIndex]?.Invoke(e.Data.Name);
        // }

        public void SetSkin(string name)
        {
            // if (Status < 0 || ske == null || ske.Data == null) return;
            // Skin skin = ske.Data.FindSkin(name);
            // if (skin != null) SetSkin(skin);
        }

        // public void SetSkin(Skin skin)
        // {
        //     if (ske == null) return;
        //     for (int i = 0; i < 5; ++i)
        //     {
        //         eventCallback[i] = null;
        //         doneCallback[i] = null;
        //     }
        //     ske.SetSkin(skin);
        //     ske.SetSlotsToSetupPose();
        //     anim.Apply(ske);
        // }

        // public int GetNumOfEvent(string name, int track = 0)
        // {
        //     if (Status < 1) return -1;
        //     Spine.Animation a = ske.Data.FindAnimation(AnimName(track));
        //     int t = 0;
        //     foreach (EventTimeline time in a.Timelines)
        //     {
        //         if (time is EventTimeline)
        //         {
        //             foreach (Spine.Event e in (time as EventTimeline).Events)
        //             {
        //                 if (e.Data.Name == name) ++t;
        //             }
        //         }
        //     }
        //     return t;
        // }

        // public float GetTimeOfEvent(string name, int track = 0)
        // {
        //     if (Status < 1) return -1;
        //     Spine.Animation a = ske.Data.FindAnimation(AnimName(track));
        //     foreach (EventTimeline time in a.Timelines)
        //     {
        //         if (time is EventTimeline)
        //         {
        //             foreach (Spine.Event e in (time as EventTimeline).Events)
        //             {
        //                 if (e.Data.Name == name) return e.Time;
        //             }
        //         }
        //     }
        //     return -1;
        // }

        // public float GetTimeOfAnim(string name)
        // {
        //     if (Status < 1 || !CheckAnim(name)) return -1;
        //     Spine.Animation a = ske.Data.FindAnimation(name);
        //     return a.Duration;
        // }

        // public void SetTime(int track, float time)
        // {
        //     if (Status < 1) return;
        //     anim.Tracks.Items[track].TrackTime = time;
        // }

        // public void ChangeSkeletonData(SkeletonDataAsset asset)
        // {
        //     if (asset == null) return;
        //     if (Status == 1 || skeAnim != null)
        //     {
        //         if (skeAnim.skeletonDataAsset == asset) return;
        //         skeAnim.skeletonDataAsset = asset;
        //         skeAnim.Initialize(true);
        //         status = 0;
        //     }
        //     else if (skeGraph != null)
        //     {
        //         if (skeGraph.skeletonDataAsset == asset) return;
        //         skeGraph.skeletonDataAsset = asset;
        //         skeGraph.Initialize(true);
        //         status = 0;
        //     }
        // }

        // public void SetAttachment(string slot, string name)
        // {
        //     if (Status < 1) return;
        //     ske.SetAttachment(slot, name);
        // }

        // public void ChangeAssets(string slot, string name, Sprite spr)
        // {
        //     if (Status < 1) return;
        //     SkeletonDataAsset dataAsset = Status == 1 ? skeAnim.SkeletonDataAsset : skeGraph.SkeletonDataAsset;
        //     int slotIndex = ske.Data.FindSlot(slot).Index;
        //     Material sourceMaterial = dataAsset.atlasAssets[0].PrimaryMaterial;

        //     Attachment attachment = ske.Skin.GetAttachment(slotIndex, name).GetRemappedClone(spr, sourceMaterial, true, true, true);
        //     Skin tempSkin = new Skin("Temp");
        //     tempSkin.SetAttachment(slotIndex, name, attachment);

        //     Skin packSkin = new Skin("Pack");
        //     packSkin.AddSkin(ske.Data.DefaultSkin);
        //     packSkin.AddSkin(ske.Skin);
        //     packSkin.AddSkin(tempSkin);

        //     Material runtimeMaterial;
        //     Texture2D runtimeAtlas;

        //     SetSkin(packSkin.GetRepackedSkin("Repacked", sourceMaterial, out runtimeMaterial, out runtimeAtlas));
        // }

        // public void Fade(float value)
        // {
        //     if (Status < 1) return;
        //     foreach (Slot slot in ske.Slots)
        //         slot.A = value;
        // }
    }
}