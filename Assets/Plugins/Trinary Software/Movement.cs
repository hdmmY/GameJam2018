using System.Collections.Generic;
using UnityEngine;
using MEC;

#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif

// /////////////////////////////////////////////////////////////////////////////////////////
//                                      MOVEMENT / TIME
// 
// This libarary is for creating movement in GUI elements, game objects, cameras, or nearly
// anything else, set it and forget it style. 
// 
// Created by Teal Rogers
// Trinary Software
// trinaryllc@gmail.com
// All rights preserved.
// /////////////////////////////////////////////////////////////////////////////////////////

namespace MovementEffects
{
    public class Movement : MonoBehaviour
    {
        //public static bool PoolInstanceVariables
        //{
        //    get { return Instance.poolInstanceVariables; }
        //    set { Instance.poolInstanceVariables = value; }
        //}

        private static Movement _instance;
        public static Movement Instance
        {
            get
            {
                if (null == _instance || !_instance.gameObject)
                {
                    GameObject instanceHome = GameObject.Find("Timing Controller") ?? new GameObject { name = "Timing Controller" };
                    _instance = instanceHome.GetComponent<Movement>() ?? instanceHome.AddComponent<Movement>();
                }

                return _instance;
            }
            set { _instance = value; }
        }

        void Awake()
        {
            if (_instance == null)
                _instance = this;
        }

        void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }

        private static Queue<MovementOverTime.PrivateSequenceInstance<float>> InstancePoolFloat = new Queue<MovementOverTime.PrivateSequenceInstance<float>>();
        private static Queue<MovementOverTime.PrivateSequenceInstance<double>> InstancePoolDouble = new Queue<MovementOverTime.PrivateSequenceInstance<double>>();
        private static Queue<MovementOverTime.PrivateSequenceInstance<Vector2>> InstancePoolVector2 = new Queue<MovementOverTime.PrivateSequenceInstance<Vector2>>();
        private static Queue<MovementOverTime.PrivateSequenceInstance<Vector3>> InstancePoolVector3 = new Queue<MovementOverTime.PrivateSequenceInstance<Vector3>>();
        private static Queue<MovementOverTime.PrivateSequenceInstance<Vector4>> InstancePoolVector4 = new Queue<MovementOverTime.PrivateSequenceInstance<Vector4>>();
        private static Queue<MovementOverTime.PrivateSequenceInstance<Rect>> InstancePoolRect = new Queue<MovementOverTime.PrivateSequenceInstance<Rect>>();
        private static Queue<MovementOverTime.PrivateSequenceInstance<Color>> InstancePoolColor = new Queue<MovementOverTime.PrivateSequenceInstance<Color>>();
        private static Queue<MovementOverTime.PrivateSequenceInstance<Quaternion>> InstancePoolQuaternion = new Queue<MovementOverTime.PrivateSequenceInstance<Quaternion>>();

        private const short PoolCheckFrequency = 64;
        private short _skipFrames = PoolCheckFrequency;

        public int QueuedFloatSequences;
        public int QueuedDoubleSequences;
        public int QueuedVector2Sequences;
        public int QueuedVector3Sequences;
        public int QueuedVector4Sequences;
        public int QueuedRectSequences;
        public int QueuedColorSequences;
        public int QueuedQuaternionSequences;

        private int _qFloatSequences;
        private int _qDoubleSequences;
        private int _qVector2Sequences;
        private int _qVector3Sequences;
        private int _qVector4Sequences;
        private int _qRectSequences;
        private int _qColorSequences;
        private int _qQuaternionSequences;

        public IEnumerator<float> _UpdateStats()
        {
            while (Instance.gameObject != null)
            {
                QueuedFloatSequences = InstancePoolFloat.Count;
                if (QueuedFloatSequences < _qFloatSequences) _qFloatSequences = 0;
                QueuedDoubleSequences = InstancePoolDouble.Count;
                if (QueuedDoubleSequences < _qDoubleSequences) _qDoubleSequences = 0;
                QueuedVector2Sequences = InstancePoolVector2.Count;
                if (QueuedVector2Sequences < _qVector2Sequences) _qVector2Sequences = 0;
                QueuedVector3Sequences = InstancePoolVector3.Count;
                if (QueuedVector3Sequences < _qVector3Sequences) _qVector3Sequences = 0;
                QueuedVector4Sequences = InstancePoolVector4.Count;
                if (QueuedVector4Sequences < _qVector4Sequences) _qVector4Sequences = 0;
                QueuedRectSequences = InstancePoolRect.Count;
                if (QueuedRectSequences < _qRectSequences) _qRectSequences = 0;
                QueuedColorSequences = InstancePoolColor.Count;
                if (QueuedColorSequences < _qColorSequences) _qColorSequences = 0;
                QueuedQuaternionSequences = InstancePoolQuaternion.Count;
                if (QueuedQuaternionSequences < _qQuaternionSequences) _qQuaternionSequences = 0;

                if (--_skipFrames < 0)
                {
                    _skipFrames = PoolCheckFrequency;

                    if (_qFloatSequences > 0) InstancePoolFloat.Clear();
                    _qFloatSequences = InstancePoolFloat.Count;
                    if (_qDoubleSequences > 0) InstancePoolDouble.Clear();
                    _qDoubleSequences = InstancePoolDouble.Count;
                    if (_qVector2Sequences > 0) InstancePoolVector2.Clear();
                    _qVector2Sequences = InstancePoolVector2.Count;
                    if (_qVector3Sequences > 0) InstancePoolVector3.Clear();
                    _qVector3Sequences = InstancePoolVector3.Count;
                    if (_qVector4Sequences > 0) InstancePoolVector4.Clear();
                    _qVector4Sequences = InstancePoolVector4.Count;
                    if (_qRectSequences > 0) InstancePoolRect.Clear();
                    _qRectSequences = InstancePoolRect.Count;
                    if (_qColorSequences > 0) InstancePoolColor.Clear();
                    _qColorSequences = InstancePoolColor.Count;
                    if (_qQuaternionSequences > 0) InstancePoolQuaternion.Clear();
                    _qQuaternionSequences = InstancePoolQuaternion.Count;
                }

                yield return 0f;
            }
        }

        public static void ClearInstancePools()
        {
            InstancePoolFloat.Clear();
            InstancePoolDouble.Clear();
            InstancePoolVector2.Clear();
            InstancePoolVector3.Clear();
            InstancePoolVector4.Clear();
            InstancePoolRect.Clear();
            InstancePoolColor.Clear();
            InstancePoolQuaternion.Clear();
        }

        /// <summary>
        /// Runs the specified sequence.
        /// </summary>
        /// <param name="sequence">The sequence to run.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<float> Run<TRef>(Sequence<TRef, float> sequence, SequenceInstance<float> legacy = null)
        {
            Profiler.BeginSample("Start float Sequence");
            Profiler.BeginSample("Fixing Any Issues");
            if (!MovementOverTime.SanatizeSequence(ref sequence))
            {
                Profiler.EndSample();
                Profiler.EndSample();
                return null;
            }
            Profiler.EndSample();

            MovementOverTime.PrivateSequenceInstance<float> inst = null;

            if (InstancePoolFloat.Count >= 1)
            {
                Profiler.BeginSample("Using Pooled Instance");
                inst = InstancePoolFloat.Dequeue();

                inst.Recycle(legacy);
                inst.Inertia = sequence.Inertia;
                inst.Elasticity = sequence.Elasticity;
                Profiler.EndSample();
            }

            if (inst == null)
            {
                Profiler.BeginSample("Using New Instance");
                inst = new MovementOverTime.PrivateSequenceInstance<float>(legacy)
                {
                    Inertia = sequence.Inertia,
                    Elasticity = sequence.Elasticity,
                    InstancePool = InstancePoolFloat,
                };
                Profiler.EndSample();
            }

            Profiler.BeginSample("Adding Process To Queue");
            inst.Process = MovementOverTime._RunSequence(sequence, inst);
            Timing.RunCoroutine(inst.Process, (Segment)sequence.Segment);
            Profiler.EndSample();

            Profiler.EndSample();

            return inst;
        }

        /// <summary>
        /// Runs the specified sequence.
        /// </summary>
        /// <param name="sequence">The sequence to run.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<double> Run<TRef>(Sequence<TRef, double> sequence, SequenceInstance<double> legacy = null)
        {
            Profiler.BeginSample("Start double Sequence");

            Profiler.BeginSample("Fixing Any Issues");
            if (!MovementOverTime.SanatizeSequence(ref sequence))
            {
                Profiler.EndSample();
                Profiler.EndSample();
                return null;
            }
            Profiler.EndSample();

            MovementOverTime.PrivateSequenceInstance<double> inst = null;

            if (InstancePoolDouble.Count >= 1)
            {
                Profiler.BeginSample("Using Pooled Instance");

                inst = InstancePoolDouble.Dequeue();

                inst.Recycle(legacy);
                inst.Inertia = sequence.Inertia;
                inst.Elasticity = sequence.Elasticity;

                Profiler.EndSample();
            }

            if (inst == null)
            {
                Profiler.BeginSample("Using New Instance");

                inst = new MovementOverTime.PrivateSequenceInstance<double>(legacy)
                {
                    Inertia = sequence.Inertia,
                    Elasticity = sequence.Elasticity,
                    InstancePool = InstancePoolDouble,
                };

                Profiler.EndSample();
            }

            Profiler.BeginSample("Adding Process To Queue");

            inst.Process = MovementOverTime._RunSequence(sequence, inst);
            Timing.RunCoroutine(inst.Process, (Segment)sequence.Segment);

            Profiler.EndSample();

            Profiler.EndSample();
            return inst;
        }

        /// <summary>
        /// Runs the specified sequence.
        /// </summary>
        /// <param name="sequence">The sequence to run.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Vector2> Run<TRef>(Sequence<TRef, Vector2> sequence, SequenceInstance<Vector2> legacy = null)
        {
            Profiler.BeginSample("Start Vector2 Sequence");

            Profiler.BeginSample("Fixing Any Issues");
            if (!MovementOverTime.SanatizeSequence(ref sequence))
            {
                Profiler.EndSample();
                Profiler.EndSample();
                return null;
            }
            Profiler.EndSample();

            MovementOverTime.PrivateSequenceInstance<Vector2> inst = null;

            if (InstancePoolVector2.Count >= 1)
            {
                Profiler.BeginSample("Using Pooled Instance");

                inst = InstancePoolVector2.Dequeue();

                inst.Recycle(legacy);
                inst.Inertia = sequence.Inertia;
                inst.Elasticity = sequence.Elasticity;

                Profiler.EndSample();
            }

            if (inst == null)
            {
                Profiler.BeginSample("Using New Instance");

                inst = new MovementOverTime.PrivateSequenceInstance<Vector2>(legacy)
                {
                    Inertia = sequence.Inertia,
                    Elasticity = sequence.Elasticity,
                    InstancePool = InstancePoolVector2,
                };

                Profiler.EndSample();
            }

            Profiler.BeginSample("Adding Process To Queue");

            inst.Process = MovementOverTime._RunSequence(sequence, inst);
            Timing.RunCoroutine(inst.Process, (Segment)sequence.Segment);

            Profiler.EndSample();

            Profiler.EndSample();
            return inst;
        }

        /// <summary>
        /// Runs the specified sequence.
        /// </summary>
        /// <param name="sequence">The sequence to run.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Vector3> Run<TRef>(Sequence<TRef, Vector3> sequence, SequenceInstance<Vector3> legacy = null)
        {
            Profiler.BeginSample("Start Vector3 Sequence");

            Profiler.BeginSample("Fixing Any Issues");
            if (!MovementOverTime.SanatizeSequence(ref sequence))
            {
                Profiler.EndSample();
                Profiler.EndSample();
                return null;
            }
            Profiler.EndSample();

            MovementOverTime.PrivateSequenceInstance<Vector3> inst = null;

            if (InstancePoolVector3.Count >= 1)
            {
                Profiler.BeginSample("Using Pooled Instance");

                inst = InstancePoolVector3.Dequeue();

                inst.Recycle(legacy);
                inst.Inertia = sequence.Inertia;
                inst.Elasticity = sequence.Elasticity;

                Profiler.EndSample();
            }

            if (inst == null)
            {
                Profiler.BeginSample("Using New Instance");

                inst = new MovementOverTime.PrivateSequenceInstance<Vector3>(legacy)
                {
                    Inertia = sequence.Inertia,
                    Elasticity = sequence.Elasticity,
                    InstancePool = InstancePoolVector3,
                };

                Profiler.EndSample();
            }

            Profiler.BeginSample("Adding Process To Queue");

            inst.Process = MovementOverTime._RunSequence(sequence, inst);
            Timing.RunCoroutine(inst.Process, (Segment)sequence.Segment);

            Profiler.EndSample();

            Profiler.EndSample();
            return inst;
        }

        /// <summary>
        /// Runs the specified sequence.
        /// </summary>
        /// <param name="sequence">The sequence to run.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Vector4> Run<TRef>(Sequence<TRef, Vector4> sequence, SequenceInstance<Vector4> legacy = null)
        {
            Profiler.BeginSample("Start Vector4 Sequence");

            Profiler.BeginSample("Fixing Any Issues");
            if (!MovementOverTime.SanatizeSequence(ref sequence))
            {
                Profiler.EndSample();
                Profiler.EndSample();
                return null;
            }
            Profiler.EndSample();

            MovementOverTime.PrivateSequenceInstance<Vector4> inst = null;

            if (InstancePoolVector4.Count >= 1)
            {
                Profiler.BeginSample("Using Pooled Instance");

                inst = InstancePoolVector4.Dequeue();

                inst.Recycle(legacy);
                inst.Inertia = sequence.Inertia;
                inst.Elasticity = sequence.Elasticity;

                Profiler.EndSample();
            }

            if (inst == null)
            {
                Profiler.BeginSample("Using New Instance");

                inst = new MovementOverTime.PrivateSequenceInstance<Vector4>(legacy)
                {
                    Inertia = sequence.Inertia,
                    Elasticity = sequence.Elasticity,
                    InstancePool = InstancePoolVector4,
                };

                Profiler.EndSample();
            }

            Profiler.BeginSample("Adding Process To Queue");

            inst.Process = MovementOverTime._RunSequence(sequence, inst);
            Timing.RunCoroutine(inst.Process, (Segment)sequence.Segment);

            Profiler.EndSample();

            Profiler.EndSample();
            return inst;
        }

        /// <summary>
        /// Runs the specified sequence.
        /// </summary>
        /// <param name="sequence">The sequence to run.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Rect> Run<TRef>(Sequence<TRef, Rect> sequence, SequenceInstance<Rect> legacy = null)
        {
            Profiler.BeginSample("Start Rect Sequence");

            Profiler.BeginSample("Fixing Any Issues");
            if (!MovementOverTime.SanatizeSequence(ref sequence))
            {
                Profiler.EndSample();
                Profiler.EndSample();
                return null;
            }
            Profiler.EndSample();

            MovementOverTime.PrivateSequenceInstance<Rect> inst = null;

            if (InstancePoolRect.Count >= 1)
            {
                Profiler.BeginSample("Using Pooled Instance");

                inst = InstancePoolRect.Dequeue();

                inst.Recycle(legacy);
                inst.Inertia = sequence.Inertia;
                inst.Elasticity = sequence.Elasticity;

                Profiler.EndSample();
            }

            if (inst == null)
            {
                Profiler.BeginSample("Using New Instance");

                inst = new MovementOverTime.PrivateSequenceInstance<Rect>(legacy)
                {
                    Inertia = sequence.Inertia,
                    Elasticity = sequence.Elasticity,
                    InstancePool = InstancePoolRect,
                };

                Profiler.EndSample();
            }

            Profiler.BeginSample("Adding Process To Queue");

            inst.Process = MovementOverTime._RunSequence(sequence, inst);
            Timing.RunCoroutine(inst.Process, (Segment)sequence.Segment);

            Profiler.EndSample();

            Profiler.EndSample();
            return inst;
        }

        /// <summary>
        /// Runs the specified sequence.
        /// </summary>
        /// <param name="sequence">The sequence to run.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Color> Run<TRef>(Sequence<TRef, Color> sequence, SequenceInstance<Color> legacy = null)
        {
            Profiler.BeginSample("Start Color Sequence");

            Profiler.BeginSample("Fixing Any Issues");
            if (!MovementOverTime.SanatizeSequence(ref sequence))
            {
                Profiler.EndSample();
                Profiler.EndSample();
                return null;
            }
            Profiler.EndSample();

            MovementOverTime.PrivateSequenceInstance<Color> inst = null;

            if (InstancePoolColor.Count >= 1)
            {
                Profiler.BeginSample("Using Pooled Instance");

                inst = InstancePoolColor.Dequeue();

                inst.Recycle(legacy);
                inst.Inertia = sequence.Inertia;
                inst.Elasticity = sequence.Elasticity;

                Profiler.EndSample();
            }

            if (inst == null)
            {
                Profiler.BeginSample("Using New Instance");

                inst = new MovementOverTime.PrivateSequenceInstance<Color>(legacy)
                {
                    Inertia = sequence.Inertia,
                    Elasticity = sequence.Elasticity,
                    InstancePool = InstancePoolColor,
                };

                Profiler.EndSample();
            }

            Profiler.BeginSample("Adding Process To Queue");

            inst.Process = MovementOverTime._RunSequence(sequence, inst);
            Timing.RunCoroutine(inst.Process, (Segment)sequence.Segment);

            Profiler.EndSample();

            Profiler.EndSample();
            return inst;
        }

        /// <summary>
        /// Runs the specified sequence.
        /// </summary>
        /// <param name="sequence">The sequence to run.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Quaternion> Run<TRef>(Sequence<TRef, Quaternion> sequence, SequenceInstance<Quaternion> legacy = null)
        {
            Profiler.BeginSample("Start Quaternion Sequence");

            Profiler.BeginSample("Fixing Any Issues");
            if (!MovementOverTime.SanatizeSequence(ref sequence))
            {
                Profiler.EndSample();
                Profiler.EndSample();
                return null;
            }
            Profiler.EndSample();

            MovementOverTime.PrivateSequenceInstance<Quaternion> inst = null;

            if (InstancePoolQuaternion.Count >= 1)
            {
                Profiler.BeginSample("Using Pooled Instance");

                inst = InstancePoolQuaternion.Dequeue();

                inst.Recycle(legacy);
                inst.Inertia = sequence.Inertia;
                inst.Elasticity = sequence.Elasticity;

                Profiler.EndSample();
            }

            if (inst == null)
            {
                Profiler.BeginSample("Using New Instance");

                inst = new MovementOverTime.PrivateSequenceInstance<Quaternion>(legacy)
                {
                    Inertia = sequence.Inertia,
                    Elasticity = sequence.Elasticity,
                    InstancePool = InstancePoolQuaternion,
                };

                Profiler.EndSample();
            }

            Profiler.BeginSample("Adding Process To Queue");

            inst.Process = MovementOverTime._RunSequence(sequence, inst);
            Timing.RunCoroutine(inst.Process, (Segment)sequence.Segment);

            Profiler.EndSample();

            Profiler.EndSample();

            return inst;
        }

        /// <summary>
        /// Runs the specified effect.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied actions.</param>
        /// <param name="sequence">The sequence to run.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.e</param>
        public static SequenceInstance<float> Run<TRef>(TRef reference, Sequence<TRef, float> sequence, SequenceInstance<float> legacy = null)
        {
            sequence.Reference = reference;
            return Run(sequence, legacy);
        }

        /// <summary>
        /// Runs the specified effect.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied actions.</param>
        /// <param name="sequence">The sequence to run.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<double> Run<TRef>(TRef reference, Sequence<TRef, double> sequence, SequenceInstance<double> legacy = null)
        {
            sequence.Reference = reference;
            return Run(sequence, legacy);
        }

        /// <summary>
        /// Runs the specified effect.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied actions.</param>
        /// <param name="sequence">The sequence to run.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Vector2> Run<TRef>(TRef reference, Sequence<TRef, Vector2> sequence, SequenceInstance<Vector2> legacy = null)
        {
            sequence.Reference = reference;
            return Run(sequence, legacy);
        }

        /// <summary>
        /// Runs the specified effect.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied actions.</param>
        /// <param name="sequence">The sequence to run.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Vector3> Run<TRef>(TRef reference, Sequence<TRef, Vector3> sequence, SequenceInstance<Vector3> legacy = null)
        {
            sequence.Reference = reference;
            return Run(sequence, legacy);
        }

        /// <summary>
        /// Runs the specified effect.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied actions.</param>
        /// <param name="sequence">The sequence to run.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Vector4> Run<TRef>(TRef reference, Sequence<TRef, Vector4> sequence, SequenceInstance<Vector4> legacy = null)
        {
            sequence.Reference = reference;
            return Run(sequence, legacy);
        }

        /// <summary>
        /// Runs the specified effect.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied actions.</param>
        /// <param name="sequence">The sequence to run.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Rect> Run<TRef>(TRef reference, Sequence<TRef, Rect> sequence, SequenceInstance<Rect> legacy = null)
        {
            sequence.Reference = reference;
            return Run(sequence, legacy);
        }

        /// <summary>
        /// Runs the specified effect.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied actions.</param>
        /// <param name="sequence">The sequence to run.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Color> Run<TRef>(TRef reference, Sequence<TRef, Color> sequence, SequenceInstance<Color> legacy = null)
        {
            sequence.Reference = reference;
            return Run(sequence, legacy);
        }

        /// <summary>
        /// Runs the specified effect.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied actions.</param>
        /// <param name="sequence">The sequence to run.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Quaternion> Run<TRef>(TRef reference, Sequence<TRef, Quaternion> sequence, SequenceInstance<Quaternion> legacy = null)
        {
            sequence.Reference = reference;
            return Run(sequence, legacy);
        }

        /// <summary>
        /// Runs the specified effect.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied actions.</param>
        /// <param name="effect">The effect to run.</param>
        /// <param name="startVal">The value that will be passed in as the second parameter to RetrieveStart.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<float> Run<TRef>(TRef reference, Effect<TRef, float> effect,
            System.Func<TRef, float> startVal = null, SequenceInstance<float> legacy = null)
        {
            return Run(new Sequence<TRef, float> { Reference = reference, Effects = new[] { effect }, RetrieveSequenceStart = startVal }, legacy);
        }

        /// <summary>
        /// Runs the specified effect.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied actions.</param>
        /// <param name="effect">The effect to run.</param>
        /// <param name="startVal">The value that will be passed in as the second parameter to RetrieveStart.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<double> Run<TRef>(TRef reference, Effect<TRef, double> effect,
            System.Func<TRef, double> startVal = null, SequenceInstance<double> legacy = null)
        {
            return Run(new Sequence<TRef, double> { Reference = reference, Effects = new[] { effect }, RetrieveSequenceStart = startVal }, legacy);
        }

        /// <summary>
        /// Runs the specified effect.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied actions.</param>
        /// <param name="effect">The effect to run.</param>
        /// <param name="startVal">The value that will be passed in as the second parameter to RetrieveStart.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Vector2> Run<TRef>(TRef reference, Effect<TRef, Vector2> effect,
            System.Func<TRef, Vector2> startVal = null, SequenceInstance<Vector2> legacy = null)
        {
            return Run(new Sequence<TRef, Vector2> { Reference = reference, Effects = new[] { effect }, RetrieveSequenceStart = startVal }, legacy);
        }

        /// <summary>
        /// Runs the specified effect.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied actions.</param>
        /// <param name="effect">The effect to run.</param>
        /// <param name="startVal">The value that will be passed in as the second parameter to RetrieveStart.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Vector3> Run<TRef>(TRef reference, Effect<TRef, Vector3> effect,
            System.Func<TRef, Vector3> startVal = null, SequenceInstance<Vector3> legacy = null)
        {
            return Run(new Sequence<TRef, Vector3> { Reference = reference, Effects = new[] { effect }, RetrieveSequenceStart = startVal }, legacy);
        }

        /// <summary>
        /// Runs the specified effect.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied actions.</param>
        /// <param name="effect">The effect to run.</param>
        /// <param name="startVal">The value that will be passed in as the second parameter to RetrieveStart.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Vector4> Run<TRef>(TRef reference, Effect<TRef, Vector4> effect,
            System.Func<TRef, Vector4> startVal = null, SequenceInstance<Vector4> legacy = null)
        {
            return Run(new Sequence<TRef, Vector4> { Reference = reference, Effects = new[] { effect }, RetrieveSequenceStart = startVal }, legacy);
        }

        /// <summary>
        /// Runs the specified effect.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied actions.</param>
        /// <param name="effect">The effect to run.</param>
        /// <param name="startVal">The value that will be passed in as the second parameter to RetrieveStart.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Rect> Run<TRef>(TRef reference, Effect<TRef, Rect> effect,
            System.Func<TRef, Rect> startVal = null, SequenceInstance<Rect> legacy = null)
        {
            return Run(new Sequence<TRef, Rect> { Reference = reference, Effects = new[] { effect }, RetrieveSequenceStart = startVal }, legacy);
        }

        /// <summary>
        /// Runs the specified effect.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied actions.</param>
        /// <param name="effect">The effect to run.</param>
        /// <param name="startVal">The value that will be passed in as the second parameter to RetrieveStart.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Color> Run<TRef>(TRef reference, Effect<TRef, Color> effect,
            System.Func<TRef, Color> startVal = null, SequenceInstance<Color> legacy = null)
        {
            return Run(new Sequence<TRef, Color> { Reference = reference, Effects = new[] { effect }, RetrieveSequenceStart = startVal }, legacy);
        }

        /// <summary>
        /// Runs the specified effect.
        /// </summary>
        /// <param name="reference">A value that will be passed in to the supplied actions.</param>
        /// <param name="effect">The effect to run.</param>
        /// <param name="startVal">The value that will be passed in as the second parameter to RetrieveStart.</param>
        /// <param name="legacy">If supplied, the new effect will pick up where the legacy effect left off.</param>
        public static SequenceInstance<Quaternion> Run<TRef>(TRef reference, Effect<TRef, Quaternion> effect,
            System.Func<TRef, Quaternion> startVal = null, SequenceInstance<Quaternion> legacy = null)
        {
            return Run(new Sequence<TRef, Quaternion> { Reference = reference, Effects = new[] { effect }, RetrieveSequenceStart = startVal }, legacy);
        }

    }
}
