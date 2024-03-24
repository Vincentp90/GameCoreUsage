This was an attempt at making an application that could accurately estimate how many CPU cores a game is effectively using. This by repeatedly turning on/off thread affinity until adding a single thread no longer improved FPS. However in testing I could not get this to work properly because the FPS kept fluctuating to much.
Also there doesn't seem much point to it anymore with CPUs having such high core counts nowadays and efficiency cores muddying the waters even more than hyperthreading already did.

Based on https://github.com/nefarius/Indicium-Supra
