import { TestBed } from '@angular/core/testing';

import { MainIdentDeactivateUserPageGuard } from './main-ident-deactivate-user-page.guard';

describe('MainIdentDeactivateUserPageGuard', () => {
  let guard: MainIdentDeactivateUserPageGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(MainIdentDeactivateUserPageGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
