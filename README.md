# CSS To Unity UI


This plugin allows you to use a CSS code to automatically adjust the Unity's UI objects, instead of manually set each parameter in the UI components according to the design sheet.


Instructions:

- Set your screen size according to the design screen size.

- Create a UI object (Image, Text, Button, etc...).

- Add the CSSToUnityUI.cs component to object.

- Paste the CSS code to the CSS text area.

- Hit the "Convert CSS to Unity UI" button.


Notes:

- Not all design parameters are supported.
- The screen size in the editor should be the same as in the original design.
- If working on a Prefab, It's recommended to work in Prefab Edit Mode since the plugin temporarily modifies the object's parent which is not possible from outside of the prefab.
