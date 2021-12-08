Cypress.Commands.add('login', () => {
  cy.request({
    method: 'POST',
    url: 'https://localhost:5001/connect/token',
    form: true,
    body: {
      client_id: 'veterinary-cypress',
      grant_type: 'password',
      username: 'manager@veterinary.hu',
      password: 'Aa1234.',
      scope: 'openid api-openid',
    },
  }).then((resp) => {
    console.log(resp);
    window.sessionStorage.setItem('access_token', resp.body.access_token);
  });
});
