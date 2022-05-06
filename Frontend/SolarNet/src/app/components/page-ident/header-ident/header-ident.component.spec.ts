import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderIdentComponent } from './header-ident.component';

describe('HeaderIdentComponent', () => {
  let component: HeaderIdentComponent;
  let fixture: ComponentFixture<HeaderIdentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HeaderIdentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HeaderIdentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
