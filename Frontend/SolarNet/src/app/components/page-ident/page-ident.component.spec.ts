import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageIdentComponent } from './page-ident.component';

describe('PageIdentComponent', () => {
  let component: PageIdentComponent;
  let fixture: ComponentFixture<PageIdentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageIdentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PageIdentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
