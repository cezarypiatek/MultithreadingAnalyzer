# MultithreadingAnalyzer
A set of Roslyn analyzers related to multithreading

All potential issues diagnosed by `MultithreadingAnalyzer` are described in depth in the following articles:

- [Avoid multithreading traps with Roslyn: Lock object selection](https://cezarypiatek.github.io/post/avoid-multithreading-traps-p1/)
- [Avoid thread synchronization problems with Roslyn: Synchronization primitives traps](https://cezarypiatek.github.io/post/avoid-multithreading-traps-p2/)


## Currently implemented rules:

- MT1000: Lock on publicly accessible member
- MT1001: Lock on this reference
- MT1002: Lock on object with weak identity
- MT1003: Lock on non-readonly member
- MT1004: Lock on value type instance
- MT1010: Method level synchronization
- MT1012: Acquiring lock without guarantee of releasing
- MT1013: Releasing lock without guarantee of execution
- MT1014: Passed by value SpinLock is useless
- MT1015: Readonly SpinLock is useless
- MT1016: Replace ReaderWriterLock with ReaderWriterLockSlim

