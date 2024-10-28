import { expect, it } from 'vitest';
import App from '../App';
import { render } from '@testing-library/react';

it('renders Wars', () => {
  const { queryByText } = render(<App />);

  expect(queryByText('Wars')).toBeInTheDocument();
});

