import { useState, useEffect } from 'react';

const ContactList = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [contacts, setContacts] = useState([]);

  useEffect(() => {
    fetchContacts();
  }, []);

  const fetchContacts = async () => {
    const response = await fetch('https://localhost:5001/api/contacts');
    const data = await response.json();
    setContacts(data);
    setIsLoading(false);
  };

  return isLoading ? (
    <div>Loading...</div>
  ) : (
    <ul>
      {contacts.map((contact) => (
        <li key={contact.id}>{contact.fullName}</li>
      ))}
    </ul>
  );
};

export default ContactList;
