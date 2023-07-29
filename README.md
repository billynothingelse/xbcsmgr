Xbox Connected Storage Manager
======================================
A quickly written small and easy to use application to read and write your Xbox Live game save data. Initially created to quickly navigate through games owned by the accessing account, loading each save data accordingly
with a quickly available option to download existing data or upload new data.

### Preview
![Application Screenshot](assets/screenshot_example.png)

Introduction
------------
Accessing games using Xbox Live's save data requires authorization for the 'TitleStorage' service. This requires a device authenticated token and user token. However, the device token originally went through
a Microsoft SOAP-based stage which required more effort than it was worth. Instead, utilizing the 'wincred' storage was helpful as this is where the relevant Gaming Services on Windows caches tokens.

Why?
------------
I believe that everyone has the right to access and modify their game save data on the games that they play. Despite potential issues for cheating, there are systems and report functionality in place to help reduce,
detect and generally deal with potential bad actors.

