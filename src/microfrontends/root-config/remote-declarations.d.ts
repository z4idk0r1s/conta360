// remote-declarations.d.ts
declare module 'dashboardApp/Dashboard' {
  // Aquí declaramos el tipo del componente Dashboard.
  // Asumimos que es un componente React funcional o de clase.
  const Dashboard: React.ComponentType;
  export default Dashboard;
}

// Si hubieras expuesto más módulos desde dashboardApp, los declararías aquí también.
// Por ejemplo:
// declare module 'dashboardApp/AnotherComponent' {
//   const AnotherComponent: React.ComponentType;
//   export default AnotherComponent;
// }