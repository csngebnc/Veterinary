import '../../support/commands';

describe('UI tests', () => {
  beforeEach(() => {
    cy.login(), cy.visit('http://localhost:4200');
  });

  it('should write a note and load it after refresh', () => {
    cy.get('[data-cy=note-area]').type('Újratöltés után is megmarad, amit ide beírtam?');
    cy.get('[data-cy=saveNoteButton').click();
    cy.visit('http://localhost:4200');
    cy.get('[data-cy=note-area]').should(
      'have.value',
      'Újratöltés után is megmarad, amit ide beírtam?'
    );
  });

  it('should create a species', () => {
    cy.get('[data-cy=settingsButton').click();
    cy.get('[data-cy=speciesButton').click();

    cy.wait(1000);

    cy.get('body').then(($body) => {
      if ($body.find('#delete-macska').length > 0) {
        cy.get('#delete-macska').click();
      }
    });
    cy.wait(1000);

    cy.get('[data-cy=addSpeciesButton').click();
    cy.get('[data-cy=speciesInput').type('macska');
    cy.get('[data-cy=saveAddSpeciesButton').click();
    cy.get('#cell-macska').contains('macska');
  });

  it('should create a vaccine', () => {
    cy.get('[data-cy=settingsButton').click();
    cy.get('[data-cy=vaccinesButton').click();

    cy.wait(1000);

    cy.get('body').then(($body) => {
      if ($body.find('#delete-Influenza').length > 0) {
        cy.get('#delete-Influenza').click();
      }
    });

    cy.wait(1000);

    cy.get('[data-cy=addVaccineButton').click();
    cy.get('[data-cy=vaccineInput').type('Influenza');
    cy.get('[data-cy=saveAddVaccineButton').click();
    cy.get('#cell-Influenza').contains('Influenza');
  });

  it('should start booking an appointment', () => {
    cy.get('[data-cy=startBooking').click();
    cy.get('[data-cy=userSearchInput').type('manager');
    cy.wait(2000);
    cy.get('#user-manager').click();
    cy.get('[data-cy=startBookingAppointmentButton]').click();

    cy.url().should('contain', 'appointments/create');
  });
});
