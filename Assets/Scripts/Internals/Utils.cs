using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace OmniGlyph.Internals {
    public static class Utils {
        public static int DefaultWait = 100;
        public static int DefaultTimeout = 10000;
        public static void WaitUntil(Func<bool> checkFunc) {
            WaitUntil(checkFunc, DefaultTimeout);
        }
        public static void WaitUntil(Func<bool> checkFunc, int timeout) {
            for (int time = 0; time <= timeout; time += DefaultWait) {
                if (checkFunc.Invoke()) {
                    return;
                } else {
                    Thread.Sleep(DefaultWait);
                }
            }
        }
        public static IEnumerator WaitUntilCoroutine(Func<bool> checkFunc) {
            return WaitUntilCoroutine(checkFunc, DefaultTimeout);
        }
        public static IEnumerator WaitUntilCoroutine(Func<bool> checkFunc, int timeout) {
            for (int time = 0; time <= timeout; time += DefaultWait) {
                if (checkFunc.Invoke()) {
                    yield break;
                } else {
                    yield return new WaitForSeconds(DefaultWait / 1000);
                }
            }
        }


    }
}
