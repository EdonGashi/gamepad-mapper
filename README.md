# Work in progress

.NET implementation of [Gopher360](https://github.com/Tylemagne/Gopher360), with an aim of being customizable and offering an OSD assistant.

Default profile:

Key|Function|Comment
---|---|---
A|Map LBUTTON|Button acts as a left mouse button.
B|Press ESCAPE, hold ALT+F4|
X|Press ENTER, hold repeat|
Y|Map RBUTTON|Button acts as a right mouse button.
DPadLeft|Press LEFT, hold repeat|
DPadUp|Press UP, hold repeat|
DPadRight|Press RIGHT, hold repeat|
DPadDown|Press DOWN, hold repeat|
LB|Show OSD|Not yet implemented.
RB|MOD|When holding MOD key buttons have different functions.
LT|Press BACKSPACE, hold repeat|Repeat rate is affected by pressure.
RT|Press SPACE, hold repeat|Repeat rate is affected by pressure.
Back|Press CTRL+ALT+TAB, hold ALT+TAB|Utilities for window switching.
Start|Press WINKEY, hold WINKEY+D|WINKEY+D minimizes all windows.

The keys have different functions when MOD button is pressed:

Key|Function|Comment
---|---|---
ModA|Map LBUTTON|Same as A to avoid confusion.
ModB|Press DELETE, hold repeat|
ModX|Press CTRL+TAB, hold CTRL+SHIFT+TAB|Browser next tab and previous tab.
ModY|Press CTRL+T, hold CTRL+W|Open and close browser tabs.
ModDPadLeft|Press ALT+LEFT, hold repeat|Browser back, VLC jump back.
ModDPadUp|Press VOLUME_UP, hold repeat|
ModDPadRight|Press ALT+RIGHT, hold repeat|Browser forward, VLC jump forward.
ModDPadDown|Press VOLUME_DOWN, hold repeat|
ModLT|Toggle OSD|Not yet implemented.
ModRB|N/A|
ModLT|Press CTRL+Z, hold repeat|Repeat rate is affected by pressure.
ModRT|Press CTRL+Y, hold repeat|Repeat rate is affected by pressure.
ModBack|Press F, hold BROWSER_REFRESH|F toggles full-screen in Youtube and VLC.
ModStart|Press CTRL+SHIFT+ESCAPE, hold SLEEP|Pressing opens task manager.
