import React, { useState } from 'react';

const SearchInput = () => {
  const [query, setQuery] = useState('');

  const handleInputChange = (e) => {
    setQuery(e.target.value);
  };

  return (
    <div className="search-input">
      <input
        type="text"
        placeholder="Search..."
        value={query}
        onChange={handleInputChange}
      />
      <button>Search</button>
    </div>
  );
};

export default SearchInput;
