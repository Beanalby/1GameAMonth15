﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace onegam_1501 {
    [RequireComponent(typeof(BoxCollider2D))]
    public class Section: MonoBehaviour {

        public Transform ForceTarget;
        public Conversation[] conversations;

        private TextBubble bubble;
        private List<Mover> movers;
        private Player player;

        int currentConversation = -1;

        public void Start() {
            bubble = Stage.Instance.GetBubble();
            movers = new List<Mover>(transform.GetComponentsInChildren<Mover>());
            player = GameObject.FindObjectOfType<Player>();
            foreach (Mover m in movers) {
                m.CanControl = false;
            }
        }

        public void Activate() {
            Camera.main.GetComponent<CamFollow>().SetMaxX(transform.position.x);
            player.CanControl = true;
            // if we have a conversation target, wait for the player
            // to hit it.  Otherwise immediately advance the converastion,
            // which will activate enemies.
            if (!ForceTarget) {
                AdvanceConversation();
            }
        }

        private void AdvanceConversation() {
            currentConversation++;
            if (currentConversation >= conversations.Length) {
                foreach (Mover m in movers) {
                    m.CanControl = true;
                }
                if (currentConversation != 0) {
                    player.CanControl = true;
                    CinemaBars.Instance.HideCinemaBars();
                }
            } else {
                bubble.Display(conversations[currentConversation].speaker,
                    conversations[currentConversation].text,
                    this.gameObject);
            }
        }

        public void AttackableDied(Attackable obj) {
            movers.Remove(obj.GetComponent<Mover>());
            if (movers.Count == 0) {
                SendMessageUpwards("SectionDone", this);
            }
        }
        public void BubbleDone() {
            AdvanceConversation();
        }

        public void ForceMoveFinished() {
            AdvanceConversation();
        }

        public void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.layer == LayerMask.NameToLayer("player")) {
                // player walked into our trigger, see if we have
                // some conversation to display
                if (conversations.Length != 0) {
                    player.SendMessage("ForceMove", this);
                    CinemaBars.Instance.ShowCinemaBars();
                    // story trigger only happens once, disable it
                    GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }

    }
}