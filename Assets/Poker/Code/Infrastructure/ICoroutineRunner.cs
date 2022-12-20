using System.Collections;
using UnityEngine;

namespace Poker.Code.Infrastructure
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator enumerator);
    }
}