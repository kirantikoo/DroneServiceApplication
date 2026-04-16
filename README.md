# Drone Service Application

A C# WPF desktop application for managing drone service jobs using a structured class library architecture.

This project was built to demonstrate object-oriented programming, desktop UI development, and separation of concerns by keeping the business logic in a reusable library and the user interface in a dedicated WPF application.

---

## Overview

The Drone Service Application is designed to manage drone service requests in an organised way.
It separates the application into two main parts:

* **DroneService** → the WPF front-end application
* **DroneServiceLib** → the reusable backend logic and service management library

This structure makes the project easier to maintain, test, and extend.

---

## Key Features

* Desktop application built with **WPF**
* Backend logic separated into a **class library**
* Service job handling and management
* Clear separation between UI and business logic
* Object-oriented design principles
* Solution-based project structure for scalability

---

## Project Structure

```text
DroneServiceApplication/
│
├── DroneService/                # WPF front-end application
│   ├── App.xaml
│   ├── App.xaml.cs
│   ├── MainWindow.xaml
│   └── MainWindow.xaml.cs
│
├── DroneServiceLib/             # Reusable backend / service logic
│
├── DroneServiceApplication.slnx
├── .gitignore
├── .gitattributes
└── README.md
```

---

## Technologies Used

* **C#**
* **WPF**
* **.NET**
* **Object-Oriented Programming (OOP)**

---

## What I Practised

This project helped me strengthen my skills in:

* Designing desktop applications with WPF
* Structuring a project into UI and backend layers
* Applying OOP concepts in a practical scenario
* Organising code into reusable components
* Building maintainable C# applications

---

## How to Run

1. Clone or download this repository
2. Open the solution file in **Visual Studio**
3. Build the solution
4. Run the `DroneService` project

---

## Future Improvements

* Add stronger input validation
* Improve UI styling and usability
* Add persistent data storage
* Expand service workflow features
* Add more testing for service logic

---

## Author

**Kiran Tikoo**
GitHub: [kirantikoo](https://github.com/kirantikoo)
