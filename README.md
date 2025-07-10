# RtlTextShaper 📋

**RtlTextShaper** is a Unity tool that allows you to write Arabic or Persian (RTL) text directly in the Unity Inspector using *logical* (unshaped) input, and automatically renders it correctly using TextMeshPro.

## 📦 Installation

1. Add the `RtlText.cs` script to any GameObject with a `TextMeshPro` component.
2. Write in text box and thats it!

## 🛠️ How to Use

1. **Add a TextMeshPro component** to a GameObject (e.g., `TextMeshProUGUI` or `TextMeshPro`).
2. **Attach the `RtlText` script** to the same GameObject.
3. In the Inspector, you’ll see a **"Text"** field under the `RtlText` component.
4. **Write your Arabic or Persian text logically** (e.g., `سلام دنیا`) in that field.
5. The script will:
   * Automatically shape and render the text correctly.
   * Set the `isRightToLeftText` property to `true` for right alignment.

## 📌 Example (Without RtlText)
Instead of doing this manually:
```csharp
_textMesh.text = RtlTextShaper.FixTextForTextMeshPro("سلام دنیا");
```

Just attach the `RtlText` component and enter `سلام دنیا` in the Inspector field. The tool handles the rest!

---
## 📦 نصب برای دوستان فارسی زبان
1. اسکریپت `RtlText.cs` را به یک گیم‌آبجکت که کامپوننت `TextMeshPro` دارد اضافه کنید.
2. داخل فیلد `text` بنویسید و تمام!
