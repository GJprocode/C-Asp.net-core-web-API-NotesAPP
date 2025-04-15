# Notes Application Frontend

This is the **frontend** of the **Notes Application**, built with **Vue.js (TypeScript)** and **Tailwind CSS**. It provides a user-friendly interface for creating, reading, updating, and deleting notes, with features like search, filtering, and sorting.

## **Features**
- **CRUD Operations**: Create, read, update, and delete notes.
- **Search and Filter**: Quickly find and organize your notes.
- **Responsive Design**: Built with Tailwind CSS for a mobile-friendly experience.
- **API Integration**: Connects seamlessly with the backend.

## **Getting Started**

### **1. Prerequisites**
To run the project locally, ensure the following tools are installed:

- **Node.js** (v16 or higher): [Download Node.js](https://nodejs.org/)
- **npm** (comes with Node.js) or **yarn**
- **Visual Studio Code**: [Download VS Code](https://code.visualstudio.com/)
  - Recommended Extensions:
    - [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) (Vue.js support)
    - [Tailwind CSS IntelliSense](https://marketplace.visualstudio.com/items?itemName=bradlc.vscode-tailwindcss) (Tailwind CSS support)

### **2. Clone the Repository**
Clone the repository and navigate to the frontend directory:
```bash
git clone https://github.com/GJprocode/NotesApp.git
cd NotesApp/frontend
```

### **3. Install Dependencies**
Install the required dependencies using `npm`:
```bash
npm install
```

### **4. Run the Development Server**
Start the frontend in development mode:
```bash
npm run dev
```

The application will be available at:
```
http://localhost:5173
```

### **5. Build for Production**
To build the project for production:
```bash
npm run build
```

This will create a production-ready build in the `dist` folder.

### **6. Backend Integration**
**Make sure the backend API is running(dotnet run) with npm run dev frontend, seperate terminals**, and update the API URL in the frontend if necessary. Modify the `src/utils/api.ts` or relevant configuration file to point to your backend:
```typescript
const API_BASE_URL = 'http://localhost:5000/api';
```

### **7. Directory Structure**
```
frontend/
├── public/            # Static assets
├── src/
│   ├── components/    # Reusable components
│   ├── views/         # Page-level components .Vue files
│   ├── utils/         # Utility functions (e.g., API integration)
│   └── main.ts        # Entry point
├── tailwind.config.js # Tailwind CSS configuration
└── vite.config.ts     # Vite configuration
```

## **Notes**

- Ensure the backend API is running to test the full functionality, rebuild frequently for chnaging to take affect.
- Use the `npm run lint` command to check for linting issues.
- All secrets (like API keys) should be handled securely and excluded from the repository.
