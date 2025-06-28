
# 🔐 Fortifying Data Privacy with k-Anonymity, l-Diversity & t-Closeness

![Language: C#](https://img.shields.io/badge/language-C%23-blue)
![Platform: Windows Forms](https://img.shields.io/badge/platform-Windows--Forms-green)

> A privacy-preserving desktop tool implementing **k-Anonymity**, **l-Diversity**, and **t-Closeness** models to anonymize sensitive data without compromising its analytical value.

---

## 📖 Project Overview

This project offers a Windows Forms application built with **C#** that supports practical anonymization of datasets through three industry-standard privacy models: **k-Anonymity**, **l-Diversity**, and **t-Closeness**. It enables organizations, researchers, and students to protect individual privacy in tabular data by minimizing the risk of re-identification while maintaining data utility for analysis.

---

## 🔐 Why Data Privacy & Anonymization Matter

Protecting personal data has become crucial in today’s digital world, where vast amounts of sensitive information are collected, shared, and analyzed daily. Effective anonymization ensures that datasets can be used for research, analytics, or public release **without exposing individual identities or sensitive attributes**—reducing risks like **identity theft, discrimination, or reputational harm** 
### 🛡️ Key Benefits

- **Regulatory Compliance**  
  Helps meet legal obligations under GDPR, HIPAA, CCPA, and other data protection laws by minimizing exposure of Personally Identifiable Information (PII).

- **Maintains Data Utility**  
  Models like **k-anonymity**, **l-diversity**, and **t-closeness** strike a balance between privacy protection and analytical value—essential for scientific discovery and real-world decision-making.  

- **Trust & Ethics**  
  Builds public trust by ensuring ethical handling of sensitive data—especially in healthcare, finance, and social science research.  

- **Mitigates Re-identification Risks**  
  Simple removal of names or IDs is not enough. Combining quasi-identifiers like ZIP code, birthdate, and gender can still re-identify **up to 87%** of individuals
  Strong anonymization techniques are essential.

---

##  Anonymization Techniques in This Project

- **k-Anonymity**  
  Ensures each record is indistinguishable from at least **k–1 others** based on quasi-identifiers. 

- **l-Diversity**  
  Protects against **attribute disclosure** by requiring that each anonymized group contains at least **l distinct values** for sensitive attributes.  

- **t-Closeness**  
  Maintains the **distribution of sensitive attributes** in each group close to the overall dataset, preventing inference attacks.  

---

##  Application Features

- Intuitive **C# Windows Forms interface**
- Load CSV/tabular datasets
- Define:
  - Explicit Identifiers (EIDs)
  - Quasi-Identifiers (QIDs)
  - Sensitive Attributes
- Apply privacy models independently or in combination
- Built-in **correlation analysis**
- Export anonymized dataset

---

## 🛠️ Getting Started

### 📌 Requirements
- Windows OS
- [.NET Framework 4.7.2+](https://dotnet.microsoft.com)
- [Visual Studio](https://visualstudio.microsoft.com/)

### Installation
```bash
git clone https://github.com/Maha028/Fortifying_DataPrivacy_with_k-Anonymity_l-Diversity_t-closeness_Models.git
````

1. Open the solution file `DataArmor.sln` in Visual Studio.
2. Build the solution (`Ctrl+Shift+B`)
3. Run the application (`F5`) or launch `DataArmor.exe` from the output folder

---

## 📂 Project Structure

| Path / File                 | Description                                      |
| --------------------------- | ------------------------------------------------ |
| `DataArmor.sln`             | Visual Studio solution file                      |
| `/Form`                     | GUI components and user interaction logic        |
| `/Anonymization`            | Core privacy model implementations               |
| `/DataAnalysis`             | Utility functions for data profiling/correlation |
| `/Resources`                | Static icons/images used in UI                   |
| `bin/Release/DataArmor.exe` | Compiled Windows application                     |


