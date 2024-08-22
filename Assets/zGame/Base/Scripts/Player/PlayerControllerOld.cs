// using UnityEngine;
// using ntDev;

// // by nt.Dev93
// namespace ntDev
// {
//     public class PlayerControllerOld : BaseControl
//     {
//         [SerializeField] bool Is2D;

//         LogicControl logicControl;

//         void Start()
//         {
//             logicControl = FindObjectOfType<LogicControl>();

//             if (logicControl == null) return;
//             if (logicControl.ControllerMove != null) logicControl.ControllerMove.OnEventDragV = OnMove;
//             if (logicControl.ControllerMove != null) logicControl.ControllerMove.OnEventRelease = OnMove;
//             if (logicControl.ControllerCam != null) logicControl.ControllerCam.OnEventDragD = OnMoveCam;
//             if (logicControl.Btn != null) logicControl.Btn.OnEventTouch = OnJumpPress;
//             if (logicControl.Btn != null) logicControl.Btn.OnEventRelease = OnJumpRelease;
//         }

//         void OnMove(Vector3 v)
//         {
//             if (Is2D) v.y = v.z = 0;
//             UnityController.vMove = v;
//         }

//         void OnMove(float time)
//         {
//             UnityController.vMove = Vector3.zero;
//         }

//         protected float SpeedCamX = 15f;
//         protected float SpeedCamY = 0.2f;
//         void OnMoveCam(Vector3 d)
//         {
//             logicControl.CinemachineFreeLook.m_XAxis.Value += d.x * SpeedCamX * Ez.TimeMod;
//             logicControl.CinemachineFreeLook.m_YAxis.Value -= d.y * SpeedCamY * Ez.TimeMod;
//         }

//         bool isJumping;
//         void OnJumpPress()
//         {
//             isJumping = true;
//         }

//         void OnJumpRelease(float time)
//         {
//             isJumping = false;
//         }

//         void Update()
//         {
//             if (logicControl == null || logicControl.ControllerMove == null) return;
//             // KeyboardControl();
//             if (isJumping) UnityController.IsJumping = true;
//             if (Is2D) transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
//         }

//         protected float value = 1;
//         protected bool[] arrMove = new bool[] { false, false, false, false };
//         protected Vector3 vKeyboard;
//         protected virtual void KeyboardControl()
//         {
//             if (Input.GetKeyDown(KeyCode.W))
//                 arrMove[0] = true;
//             else if (Input.GetKeyUp(KeyCode.W))
//                 arrMove[0] = false;
//             if (Input.GetKeyDown(KeyCode.S))
//                 arrMove[1] = true;
//             else if (Input.GetKeyUp(KeyCode.S))
//                 arrMove[1] = false;
//             if (Input.GetKeyDown(KeyCode.A))
//                 arrMove[2] = true;
//             else if (Input.GetKeyUp(KeyCode.A))
//                 arrMove[2] = false;
//             if (Input.GetKeyDown(KeyCode.D))
//                 arrMove[3] = true;
//             else if (Input.GetKeyUp(KeyCode.D))
//                 arrMove[3] = false;

//             vKeyboard = Vector3.zero;
//             if (arrMove[0]) vKeyboard.y += value;
//             if (arrMove[1]) vKeyboard.y -= value;
//             if (arrMove[2]) vKeyboard.x -= value;
//             if (arrMove[3]) vKeyboard.x += value;

//             if (Is2D) vKeyboard.y = 0;
//             UnityController.vMove = vKeyboard;
//         }
//     }
// }
